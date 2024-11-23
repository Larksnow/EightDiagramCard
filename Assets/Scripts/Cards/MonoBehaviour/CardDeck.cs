using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using System.Net.WebSockets;
using TMPro;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    private List<CardDataSO> drawDeck = new();
    private List<CardDataSO> discardDeck = new();
    private List<Card> handCardObjectList = new();
    public GameObject deckPosition;
    public GameObject discardPosition;

    // 测试用
    private void Start()
    {
        InitializeDrawDeck();
        InitializeDiscardDeck();
        DrawCard(3);
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

        ShuffleDeck(drawDeck);
    }

    public void InitializeDiscardDeck()
    {
        discardDeck.Clear();
    }

    private void Update()
    {
        // 更新抽牌堆、弃牌堆数字
        UpdateDeckNumber();
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
                RefillDrawDeckFromDiscard();
            }
            CardDataSO drawedCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            var card = cardManager.GetCardFromPool().GetComponent<Card>();
            // Initialize this drawed card
            card.Init(drawedCardData);
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }

    [ContextMenu("TestDiscardCard")]
    public void TestDiscardCard()
    {
        DiscardCard(1); 
    }

    private void DiscardCard(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            if (handCardObjectList.Count == 0)
            {
                Debug.LogWarning("No cards left in hand to discard.");
                return;
            }
            var cardToDiscard = handCardObjectList[0];
            handCardObjectList.RemoveAt(0);
            discardDeck.Add(cardToDiscard.cardData);
            cardManager.DiscardCard(cardToDiscard.gameObject);
            SetCardLayout(0.2f * i);
        }
    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; ++i)
        {
            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);
            currentCard.transform.SetPositionAndRotation(cardTransform.position, cardTransform.rotation);
            // Card enlarge animation
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransform.position, 0.5f);
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };
            // Set card order
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
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
        ShuffleDeck(drawDeck);
    }

    private void ShuffleDeck(List<CardDataSO> deck)
    {
        System.Random rng = new();
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (deck[k], deck[n]) = (deck[n], deck[k]);
        }
    }

    private void UpdateDeckNumber()
    {
        deckPosition.GetComponentInChildren<TextMeshPro>().text = drawDeck.Count.ToString();
        discardPosition.GetComponentInChildren<TextMeshPro>().text = discardDeck.Count.ToString();
    }
}
