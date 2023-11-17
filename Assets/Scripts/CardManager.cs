using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public GameObject UI;
    public SlotsManagerCollider colliderName;
    SlotsManagerCollider prevName;
    public PlantCardScriptableObject plantCardScriptableObject;
    public Sprite plantSprite;
    public GameObject plantPrefab;
    public bool isOverCollider = false;
    GameObject plant;
    bool isHoldingPlant;

    public void OnDrag(PointerEventData eventData)
    {
        if (isHoldingPlant)
        {
            //Take a gameObject
            plant.GetComponent<SpriteRenderer>().sprite = plantSprite;

            if (prevName != colliderName || prevName == null)
            {
                if (!colliderName.isOccupied)
                {
                    plant.transform.position = new Vector3(0, 0, -1);
                    plant.transform.localPosition = new Vector3(0, 0, -1);
                    isOverCollider = false;
                    if (prevName != null)
                    {
                        prevName.plant = null;
                    }
                    prevName = colliderName;
                }
            }
            else
            {
                if (!colliderName.isOccupied)
                {
                    plant.transform.position = new Vector3(0, 0, -1);
                    plant.transform.localPosition = new Vector3(0, 0, -1);
                }
            }

            if (!isOverCollider)
            {
                plant.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (GameObject.FindObjectOfType<GameManager>().SunAmount >= plantCardScriptableObject.cost)
        {
            isHoldingPlant = true;
            Vector3 pos = new Vector3(0, 0, -1);
            plant = Instantiate(plantPrefab, pos, Quaternion.identity);
            plant.GetComponent<PlantManager>().thisSO = plantCardScriptableObject;
            plant.GetComponent<PlantManager>().isDragging = true;
            plant.transform.localScale = plantCardScriptableObject.size;
            plant.GetComponent<SpriteRenderer>().sprite = plantSprite;

            plant.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            Debug.Log("Not enough sun!");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHoldingPlant)
        {
            if (colliderName != null && !colliderName.isOccupied)
            {
                GameObject.FindObjectOfType<GameManager>().DeductSun(plantCardScriptableObject.cost);
                isHoldingPlant = false;
                colliderName.isOccupied = true;
                plant.tag = "Untagged";
                plant.transform.SetParent(colliderName.transform);
                plant.transform.position = new Vector3(0, 0, -1);
                plant.transform.localPosition = new Vector3(0, 0, -1);
                plant.AddComponent<BoxCollider2D>();
                plant.AddComponent<CircleCollider2D>();
                plant.GetComponent<CircleCollider2D>().isTrigger = true;

                plant.GetComponent<PlantManager>().isDragging = false;
                if (plantCardScriptableObject.isSunFlower)
                {
                    SunSpawner sunSpawner  = plant.AddComponent<SunSpawner>();
                    sunSpawner.isSunFlower = true;
                    sunSpawner.minTime = plantCardScriptableObject.sunSpawnerTemplate.minTime;
                    sunSpawner.maxTime = plantCardScriptableObject.sunSpawnerTemplate.maxTime;
                    sunSpawner.sun = plantCardScriptableObject.sunSpawnerTemplate.sun;
                }
            }
            else
            {
                isHoldingPlant = false;
                Destroy(plant);
            }
        }
    }
}
