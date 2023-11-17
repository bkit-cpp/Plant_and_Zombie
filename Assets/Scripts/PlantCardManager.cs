using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantCardManager : MonoBehaviour
{
    [Header("Cards Parameters")]
    public int amtOfCards;
    public PlantCardScriptableObject[] plantCardSO;
    public GameObject cardPrefab;
    public Transform cardHolderTransform;

    [Header("Plant Parameters")]
    public GameObject[] plantCards;
    public float cooldown;
    public int cost;
    public Texture plantIcon;

    private void Start()
    {
        amtOfCards = plantCardSO.Length;
        plantCards = new GameObject[amtOfCards];

        for (int i = 0; i < amtOfCards; i++)
        {
            AddPlantCard(i);
        }
    }

    public void AddPlantCard(int index)
    {
        GameObject card = Instantiate(cardPrefab, cardHolderTransform);
        CardManager cardManager = card.GetComponent<CardManager>();

        cardManager.plantCardScriptableObject = plantCardSO[index];
        cardManager.plantSprite = plantCardSO[index].plantSprite;
        cardManager.UI = GameObject.FindGameObjectWithTag("Canvas");

        plantCards[index] = card;

        //Getting Variables
        plantIcon = plantCardSO[index].plantIcon;
        cost = plantCardSO[index].cost;
        cooldown = plantCardSO[index].cooldown;

        //Updating UI
        card.GetComponentInChildren<RawImage>().texture = plantIcon;
        card.GetComponentInChildren<TMP_Text>().text = "" + cost;
    }
}
