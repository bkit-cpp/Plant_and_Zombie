using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public PlantCardScriptableObject thisSO;
    public Transform shootPoint;//: Điểm bắn của cây cỏ, nơi mà đạn sẽ được bắn ra
    public GameObject Bullet;//Object đại diện cho viên đạn mà cây cỏ sẽ bắn.
    public float health;
    public float damage;
    public float range;
    public float speed;
    public LayerMask zombieLayer;// LayerMask chỉ định layer của zombie để xác định mục tiêu bằng raycast
    public float fireRate;

    public bool isMine;//Biến boolean xác định xem cây cỏ có phải là của người chơi hay không
    public float growDuration;//Thời gian cây cỏ mất để phát triển.
    public GameObject explosion;// GameObject đại diện cho hiệu ứng nổ khi cây cỏ bị phá hủy
    public Sprite grownSprite;
    public float blinkingRate;
    public Sprite[] states;//Mảng chứa các sprite biểu diễn trạng thái của cây cỏ.
    public int stateCount;//Đếm trạng thái hiện tại của cây cỏ.
    public bool isGrown;

    public bool isDragging = true;//Biến boolean xác định xem cây cỏ đang được kéo hay không

    private void Start()
    {
    /* Thiết lập các thuộc tính của cây cỏ từ PlantCardScriptableObject.
    Nếu cây cỏ thuộc sở hữu của người chơi (isMine là true), 
   khởi chạy các coroutine để cập nhật trạng thái của cây cỏ và thực hiện nhấp nháy.*/
        health = thisSO.health;
        damage = thisSO.damage;
        range = thisSO.range;
        speed = thisSO.speed;
        Bullet = thisSO.Bullet;
        zombieLayer = thisSO.zombieLayer;
        fireRate = thisSO.fireRate;

        isMine = thisSO.isMine;
        growDuration = thisSO.growDuration;
        explosion = thisSO.Explosion;
        grownSprite = thisSO.grownSprite;
        blinkingRate = thisSO.blinkingRate;
        states = thisSO.mineStates;

        if (isMine)
        {
            StartCoroutine(mineStateUpdate());
            StartCoroutine(blink());
        }

        StartCoroutine(Attack());
    }

    private void Update()
    {/*Kiểm tra xem cây cỏ có mất máu xuống 0 hay không. Nếu có, hủy đối tượng cây cỏ.*/
        if (health <= 0)
        {
            //Dead
            Destroy(this.gameObject);
        }
    }

    public IEnumerator Attack()
    {
   /*Sử dụng WaitUntil để đảm bảo rằng cây cỏ không đang được kéo (isDragging là false) trước khi bắt đầu tấn công.
Chờ một khoảng thời gian (fireRate) rồi thực hiện kiểm tra raycast để xác định có zombie trong tầm bắn hay không.
Nếu có, tạo một viên đạn và thiết lập sát thương và vận tốc cho nó. Sau đó, tiếp tục coroutine để tạo ra một chuỗi các cuộc tấn công.*/
        yield return new WaitUntil(() => !isDragging);
        yield return new WaitForSeconds(fireRate);
        if (speed > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, shootPoint.right, range, zombieLayer) ;
            Debug.DrawRay(shootPoint.position, shootPoint.right, Color.red);
            if (hit)
            {
                if (hit.transform.tag == "Zombie")
                {
                    Debug.Log("Hit zombie");
                    GameObject bullet = Instantiate(Bullet, shootPoint.transform.position, Quaternion.identity);
                    bullet.GetComponent<PeaManager>().damage = damage;
                    bullet.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
                }
            }
            StartCoroutine(Attack());
        }
    }

    public IEnumerator mineStateUpdate()
    {
        /*Sử dụng WaitUntil để đảm bảo rằng cây cỏ không đang được kéo (isDragging là false) trước khi bắt đầu cập nhật trạng thái.
        Chờ một khoảng thời gian (growDuration) để xác định thời gian phát triển của cây cỏ.*/
        isGrown = false;
        yield return new WaitUntil(() => !isDragging);
        yield return new WaitForSeconds(growDuration);
        isGrown = true;
    }

    public IEnumerator blink()
    {
        /*Sử dụng WaitUntil để đảm bảo rằng cây cỏ rỗng đang được kéo (isDragging là false) trước khi bắt đầu nhấp nháy.
Thay đổi sprite của cây cỏ và chờ một khoảng thời gian (blinkingRate), sau đó tiếp tục coroutine.*/
        yield return new WaitUntil(() => !isDragging);
        this.GetComponent<SpriteRenderer>().sprite = states[stateCount];
        yield return new WaitForSeconds(blinkingRate);
        stateCount = isGrown ? stateCount == 2 ? 3 : 2 : stateCount == 1 ? 0 : 1;
        StartCoroutine(blink());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        /*Kiểm tra va chạm với zombie khi cây cỏ là của người chơi và đã phát triển.
Nếu có va chạm, gọi phương thức DealDamage trên ZombieController.*/
        if (isMine && isGrown)
        {
            if (collision.gameObject.tag == "Zombie")
            {
                collision.GetComponent<ZombieController>().DealDamage(damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isMine && isGrown)
        {
            if (collision.gameObject.tag == "Zombie")
            {
                Instantiate(explosion, this.transform);
                Destroy(this.gameObject);
            }
        }
    }

    public void Damage(float amnt)
    {
        /*Giảm máu của cây cỏ khi nó nhận sát thương.*/
        health -= amnt;
    }
}
