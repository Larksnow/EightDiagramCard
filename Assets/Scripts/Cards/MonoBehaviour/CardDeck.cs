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

    [Header("Broadcast Events")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;

    // 测试用
    private void Start()
    {
        InitializeDrawDeck();
        InitializeDiscardDeck();
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
            // No cards in both deck
            if (drawDeck.Count == 0) return;
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
            Sequence cardAnimationSequence = DOTween.Sequence();
            cardAnimationSequence
            .Append(currentCard.transform.DOScale(Vector3.one, animationTime).SetDelay(delay)).SetEase(Ease.InOutQuart)
            .Join(currentCard.transform.DOMove(cardTransform.position, animationTime)).SetEase(Ease.InOutQuart)
            .Join(currentCard.transform.DORotateQuaternion(cardTransform.rotation, animationTime)).SetEase(Ease.InOutQuart)
            .OnComplete(() =>
            {
                currentCard.isAnimating = false;
            });
            // Set card order
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
        while (handCardObjectList.Count > 0)
        {
            DiscardCard(handCardObjectList[0]); // Always process the first card
        }
        handCardObjectList.Clear();
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
    }

    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        card.isAnimating = true;
        Sequence cardAnimationSequence = DOTween.Sequence();
        cardAnimationSequence
        .Append(card.gameObject.transform.DOScale(0, animationTime).SetEase(Ease.InOutQuart)).SetDelay(0.2f)
        .Join(card.gameObject.transform.DOMove(discardPosition.position, animationTime).SetEase(Ease.InOutQuart))
        .onComplete = () =>
        {
            cardManager.DiscardCard(card.gameObject);
            card.isAnimating = false;
        };
        // Raise IntEvent to update UI number
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
        SetCardLayout(0);
    }

}
