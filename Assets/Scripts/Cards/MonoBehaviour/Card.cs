using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
   [Header("Card Components")]
   public CardDataSO cardData;
   public TextMeshPro nameText, costText, descriptionTest;
   public int cardCost;
   public SpriteRenderer cardSprite;
   
   void Start()
   {
        Init(cardData);    
   }
   public void Init(CardDataSO data)
   {
        cardData = data;
        cardSprite.sprite = data.cardSprite;
        costText.text = data.cost.ToString();
        nameText.text = data.cardName;
        descriptionTest.text = data.cardDescription;
   }
   void OnMousedDown()
   {
      Debug.Log("Card clicked");
   }
   
}
