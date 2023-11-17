using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsManagerCollider : MonoBehaviour
{
    public GameObject plant;
    public bool isOccupied = false;

    void OnMouseOver()
    {
        foreach (CardManager item in GameObject.FindObjectsOfType<CardManager>())
        {
            item.colliderName = this.GetComponent<SlotsManagerCollider>();
            item.isOverCollider = true;
        }

        if (plant == null)
        {
            if (GameObject.FindGameObjectWithTag("Plant") != null)
            {
                plant = GameObject.FindGameObjectWithTag("Plant");
                plant.transform.SetParent(this.transform);
                Vector3 pos = new Vector3(0, 0, -1);
                plant.transform.position = new Vector3(0, 0, -1);
                plant.transform.localPosition = pos;
            }
        }
        else
        {
            isOccupied = false;
        }
    }

    private void OnMouseExit()
    {
        //Destroy(plant);
    }
}
