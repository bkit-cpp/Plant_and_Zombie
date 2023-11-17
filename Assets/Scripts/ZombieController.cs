using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public ZombieScriptableObject thisZombieSO;
    public ZombieAccessoriesManager zombieAccessories;
    public float speed; //tốc độ
    public float health;//máu
    public float handHealth;
    public float currentHealth;//máu hiện tại 
    public GameObject accessory;
    public float accessoryHealth;
    public float damage;
    public float attackInterval;
    GameObject target;
    public bool isAttacking;//zombie đang thực hiện tấn công 

    public float damageDelay = 2f;

    bool isDying;

    private void Start()
    {
        speed = thisZombieSO.zombieSpeed;
        health = thisZombieSO.zombieHealth;
        accessoryHealth = thisZombieSO.accessoryHealth;
        damage = thisZombieSO.zombieDamage;
        handHealth = thisZombieSO.zombieHandHealth;
        attackInterval = thisZombieSO.attackInterval;
        currentHealth = health;
    }

    private void Update()
    {
        if (target == null)
        {
            isAttacking = false;
        }

        if (!isAttacking && !isDying)
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            this.transform.position = this.transform.position;
        }

        if (currentHealth <= handHealth && this.transform.childCount > 1)
        {
            //Add rigidbody 2d to hand
            Transform hand = this.transform.GetChild(1);

            hand.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            hand.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;

            hand.SetParent(null);

            Destroy(hand.gameObject, 1.5f);
        }

        if (currentHealth <= 0 && this.transform.childCount > 0)
        {
            isDying = true;
            //Dead
            //Add rigidbody 2d to head
            Transform head = this.transform.GetChild(0);

            head.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            head.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;

            head.SetParent(null);

            Destroy(head.gameObject, 1.5f);

            Destroy(this.GetComponent<Rigidbody2D>());

            foreach (var item in this.GetComponents<BoxCollider2D>())
            {
                Destroy(item);
            }

            Destroy(this.gameObject, 2f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)// xử lý va chạm 
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        //Detect plant collisions
        if (collision.gameObject.tag == "Plant")//Nếu phát hiện đó là plant 
        {
            isAttacking = true; //Zombie tấn công 
            target = collision.gameObject; //Đặt đối tượng bị va chạm 
            StartCoroutine(Attack());
        }
        else if (collision.gameObject.GetComponent<PlantManager>() != null) //Nếu phát hiện nó là một đối tượng trong lớp PlantManager
        {
            isAttacking = true;//Zombie tấn công 
            target = collision.gameObject;
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//Xử lý va chạm
    {
        target = null;
        isAttacking = false;
    }

    public IEnumerator Attack()
    {
        Debug.Log("Attacking...");
        //Attack Plant
        if (target != null)// Kiểm tra xem có phải Plant không 
        {
            target.GetComponent<PlantManager>().Damage(damage);//Gây sát thương cho mục tiêu 
        }
        yield return new WaitForSeconds(attackInterval);//Quãng thời gian zombie quay lại tấn công plant
        if (target != null)
        {
            StartCoroutine(Attack()); //Kích hoạt tấn công , tạo ra liên tục các đợt tấn công 
        }
    }

    public void DealDamage(float amnt)
    {
        //Audio to play
        currentHealth -= amnt;

        if (zombieAccessories != null)
        {
            zombieAccessories.TakeDamage(amnt);
        }

        StartCoroutine(DamageColor(this.gameObject.GetComponent<SpriteRenderer>()));

        foreach (Transform item in this.transform.GetComponentInChildren<Transform>())
        {
            StartCoroutine(DamageColor(item.gameObject.GetComponent<SpriteRenderer>()));
        }
    }

    public IEnumerator DamageColor(SpriteRenderer spriteRenderer)
    {
        for (int i = 0; i <= 255; i += 10)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(i, i, i);
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 255; i <= 0; i -= 10)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(i, i, i);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
