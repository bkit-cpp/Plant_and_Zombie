using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    public bool isSunFlower;

    public float minTime;
    public float maxTime;
    public float time;

    public GameObject sun;
    public Vector2 minPos;
    public Vector2 maxPos;
    Vector3 pos;

    private void Start()
    {
        time = Random.Range(minTime, maxTime);
        if (!isSunFlower)
        {
            pos.x = Random.Range(minPos.x, maxPos.x);
            pos.y = Random.Range(minPos.y, maxPos.y);
            pos.z = -1;
        }
        else
        {
            pos.x = 0;
            pos.y = 0;
            pos.z = -1;
        }
        StartCoroutine(SpawnSun());
    }

    public IEnumerator SpawnSun()
    {
        yield return new WaitForSeconds(time);
        GameObject SunObject = Instantiate(sun, pos, Quaternion.identity);

        time = Random.Range(minTime, maxTime);
        if (!isSunFlower)
        {
            pos.x = Random.Range(minPos.x, maxPos.x);
            pos.y = Random.Range(minPos.y, maxPos.y);
            pos.z = -1;
        }
        else
        {
            //If is sunflower
            Destroy(SunObject.GetComponent<Rigidbody2D>());
            pos.x = 0;
            pos.y = 0;
            pos.z = -2;

            SunObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -2);
            //SunObject.transform.parent = this.transform;
            //SunObject.transform.localPosition = new Vector3(0,0,-2);
        }
        StartCoroutine(SpawnSun());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sun")
        {
            Destroy(collision.gameObject);
        }
    }
}
