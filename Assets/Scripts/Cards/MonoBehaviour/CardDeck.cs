using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using TMPro;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    private List<CardDataSO> drawDeck = new();
    private List<CardDataSO> discardDeck = new();
    private List<Card> handCardObjectList = new();
    public Transform deckPosition;
    public Transform discardPosition;

    public GameObject deckUI;
    public GameObject discardUI;
    public float animationTime;

    // 测试用
    private void Start()
    {
        InitializeDrawDeck();
        InitializeDiscardDeck();
        DrawCard(7);
    }

    public void InitializeDrawDeck()
    {
        drawDeck.Clear();
        // Add all cards in player's deck to draw deck
        foreach (var item in cardManager.playerHoldDeck.CardDeckEntryList)
        {
            for (int i = 0; i < item.amount; ++i)
            {
                drawDeck.Add(item.cardData);
            }
        }
        //TODO:更新抽牌堆、弃牌堆数字
        ShuffDeck();
    }

    private void InitializeDiscardDeck()
    {
        discardDeck.Clear();
    }

    [ContextMenu("TestDrawCard")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
    private void DrawCard(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            if (drawDeck.Count == 0)
            {
            //TODO:更新抽牌堆、弃牌堆数字
                RefillDrawDeckFromDiscard();
                ShuffDeck();
                // TODO: 洗牌动画
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

    private void ShuffDeck()
    {
        discardDeck.Clear();
        //TODO: update UI number
        for (int i = 0; i < drawDeck.Count; ++i)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }

    private void RefillDrawDeckFromDiscard()
    {
        if (discardDeck.Count == 0)
        {
            Debug.Log("No cards in discard pile to refill draw deck.");
            return;
        }

        drawDeck.AddRange(discardDeck);
        discardDeck.Clear();
    }
    public void DiscardAllCards()
    {
        for (int i = 0; i < handCardObjectList.Count; ++i)
        {
            var cardToDiscard = handCardObjectList[0];
            DiscardCard(cardToDiscard);
            //TODO:: discard animation
        }
    }

    public void DiscardCard(Card card)
    {
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        cardManager.DiscardCard(card.gameObject);
        SetCardLayout(0);
    }

    private void UpdateDeckNumber()
    {
        deckUI.GetComponentInChildren<TextMeshPro>().text = drawDeck.Count.ToString();
        discardUI.GetComponentInChildren<TextMeshPro>().text = discardDeck.Count.ToString();
    }
}
