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
    public SpriteRenderer cardSprite, cardFrame;

    [Header("Card Original Layout")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;
    public bool isAnimating;
    public bool isAvailable;
    public bool isDraging;
    [SerializeField] private bool isReducedCost;
    public bool isMouseOver;
    public int reducedCost;
    public Player player;

    [Header("Broadcast Event")]
    public ObjectEventSO discardCardEvent;
    public IntEventSO costManaEvent;

    private PauseManager pauseManager;


    void Start()
    {
        pauseManager = PauseManager.Instance;
        Init(cardData);
    }

    public void Init(CardDataSO data, bool toHand = true, bool reducedCost = false, int reducedCostValue = 0)
    {
        cardData = data;
        cardCost = toHand ? data.cost - reducedCostValue : data.cost;
        cardSprite.sprite = data.cardSprite;
        cardFrame.color = data.color;
        costText.text = data.cost.ToString();
        costText.color = Color.white;
        nameText.text = data.cardName;
        descriptionTest.text = data.cardDescription;
        player = FindObjectOfType<Player>();
        isMouseOver = false;
        if (toHand)
        {
            isReducedCost = reducedCost;
            UpdateCardCost(reducedCostValue);
        }
    }

    public void UpdateCardPositionRotation(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pauseManager.IsPaused()) return;
        if (isAnimating || isDraging) return;
        isMouseOver = true;
        transform.position = new Vector3(originalPosition.x, -3.5f, 0);
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (pauseManager.IsPaused()) return;
        if (isAnimating || isDraging) return;
        isMouseOver = false;
        ResetCardTransform();
    }

    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExecuteCardEffect(CharacterBase target)
    {
        costManaEvent.RaiseEvent(-cardCost, this);
        ResetCardCost();
        discardCardEvent.RaiseEvent(this, this);
        // if (cardData.effects?.Count > 0)
        // {
        //     foreach (var effect in cardData.effects)
        //     {
        //         effect.Execute(target, null);
        //     }
        // }
    }

    // 保险函数
    public void SetIfReducedCost(bool isReducedCost)
    {
        this.isReducedCost = isReducedCost;
        if (isReducedCost)
            UpdateCardCost(reducedCost);
        else
            ResetCardCost();
    }
    [ContextMenu("TEst")]
    public void TEst()
    {
        UpdateCardCost(-1);
    }
    public void UpdateCardCost(int costChange)
    {
        isReducedCost = costChange < 0;
        cardCost = cardData.cost + costChange;
        if (cardCost < 0) cardCost = 0;
        costText.text = cardCost.ToString();
        CheckCardAvailable();
    }

    public void ResetCardCost()
    {
        isReducedCost = false;
        cardCost = cardData.cost;

        costText.text = cardData.cost.ToString();
        CheckCardAvailable();
    }

    public void CheckCardAvailable()
    {
        isAvailable = player.currentMana >= cardCost;
        if (cardData is QianYangDataSO)
        {
            descriptionTest.text = ((QianYangDataSO)cardData).GetUpdatedCardDescription();
        }
        UpdateCardCostColor();
    }

    private void UpdateCardCostColor()
    {
        if (isReducedCost)
            costText.color = isAvailable ? Color.green : Color.red;
        else
            costText.color = isAvailable ? Color.white : Color.red;
    }
}
