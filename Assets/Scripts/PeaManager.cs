using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaManager : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            collision.gameObject.GetComponent<ZombieController>().DealDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
