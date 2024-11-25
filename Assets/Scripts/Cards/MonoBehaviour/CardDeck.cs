using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using TMPro;
using System.Collections;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    private List<CardDataSO> drawDeck = new();
    private List<CardDataSO> discardDeck = new();
    private List<Card> handCardObjectList = new();
    public Transform deckPosition;
    public Transform discardPosition;
    public CardDeckAnimation cardDeckAnimation;

    public GameObject deckUI;
    public GameObject discardUI;
    public float animationTime;
    public Player player;

    [Header("Broadcast Events")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;

    // 测试用
    private void Start()
    {
        InitializeDrawDeck();
        InitializeDiscardDeck();
        // 测试用
        NewTurnDrawCard(5);
    }
    public void NewTurnDrawCard(int amount)
    {
        DrawCard(amount);
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
        // Max hand limit is 10
        if (handCardObjectList.Count >= 10) return;
        for (int i = 0; i < amount; ++i)
        {
            if (drawDeck.Count == 0)
            {
                RefillDrawDeckFromDiscard();
                ShuffDeck();
            }
            CardDataSO drawedCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            // Raise IntEvent to update UI number
            drawCountEvent.RaiseEvent(drawDeck.Count, this);

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
            currentCard.UpdateCardState();
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
        // Raise IntEvent to update UI number
        drawCountEvent.RaiseEvent(drawDeck.Count, this);
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
        // TODO: 洗牌动画
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
            discardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
        }
        handCardObjectList.Clear();
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
    }

    public void DiscardCard(object obj)
    {
        StartCoroutine(DiscardWithAnimation(obj as Card));
    }

    private IEnumerator DiscardWithAnimation(Card card, int index = 0)
    {
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        // 播放弃牌动画并等待完成
        yield return cardDeckAnimation.CardTransferAnimation(discardPosition, true, card, index);
        cardManager.DiscardCard(card.gameObject);
        // Raise IntEvent to update UI number
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
        SetCardLayout(0);
    }

}
