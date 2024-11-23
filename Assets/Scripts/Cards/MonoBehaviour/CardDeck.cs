using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using System.Net.WebSockets;
using Unity.PlasticSCM.Editor.WebApi;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    private List<CardDataSO> drawDeck = new();
    private List<CardDataSO> discardDeck = new();
    private List<Card> handCardObjectList = new();
    public Transform deckPosition;
    public Transform discardPosition;
    public float animationTime;

    // 测试用
    private void Start()
    {
        InitializeDrawDeck();
        DrawCard(3);
    }

    public void InitializeDrawDeck()
    {
        drawDeck.Clear();
        // Add all cards in player's deck to draw deck
        foreach (var item in cardManager.playerHoldDeck.CardDeckEntryList) 
        {
            for(int i = 0; i < item.amount; ++i)
            { 
                drawDeck.Add(item.cardData);
            }
        }

        //TODO:洗牌/更新抽牌堆、弃牌堆数字
    }

    [ContextMenu("TestDrawCard")]
    public void TestDrawCard(){
        DrawCard(1);
    }
    private void DrawCard(int amount){
        for (int i = 0; i < amount; ++i)
        {
            if(drawDeck.Count == 0)
            {
            //TODO:洗牌/更新抽牌堆、弃牌堆数字
            }
            CardDataSO drawedCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            var card = cardManager.GetCardFromPool().GetComponent<Card>();
            // Initialize this drawed card
            card.Init(drawedCardData);
            card.transform.position = deckPosition.position;
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; ++i)
        {
            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);
            // Card draw animation
            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, animationTime).SetDelay(delay);
            currentCard.transform.DOMove(cardTransform.position, animationTime).onComplete = () => currentCard.isAnimating = false;
            currentCard.transform.DORotateQuaternion(cardTransform.rotation, animationTime);
            // Set card order
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;

            // Store a copy of current card transform
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdateCardPositionRotation(cardTransform.position, cardTransform.rotation);
        }
        
    }
}
