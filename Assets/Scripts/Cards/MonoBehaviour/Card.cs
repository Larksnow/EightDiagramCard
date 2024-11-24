using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Card Components")]
    public CardDataSO cardData;
    public TextMeshPro nameText, costText, descriptionTest;
    public int cardCost;
    public SpriteRenderer cardSprite;
    [Header("Card Original Layout")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;
    public bool isAnimating;
    public bool isAvailable;
    public Player player;
    [Header("Broadcast Event")]
    public ObjectEventSO discardCardEvent;
    public IntEventSO costManaEvent;

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
        player = GameObject.FindObjectOfType<Player>();
    }

    public void UpdateCardPositionRotation(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isAnimating) return;
        transform.position = new Vector3(originalPosition.x, -3.5f, 0);
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isAnimating) return;
        ResetCardTransform();
    }

    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExecuteCardEffect(CharacterBase target)
    {
        costManaEvent.RaiseEvent(cardData.cost, this);
        discardCardEvent.RaiseEvent(this, this);
        foreach (var effect in cardData.effects)
        {
            effect.Execute(target);
        }
    }

    public void UpdateCardState()
    {
        isAvailable = player.CurrentMana >= cardData.cost;
        costText.color = isAvailable ? Color.white : Color.red;
    }
}
