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
    public Player player;
    private List<CardDataSO> drawDeck = new();
    private List<CardDataSO> discardDeck = new();
    private List<Card> handCardObjectList = new();
    public Transform deckPosition;
    public Transform discardPosition;

    public float animationTime;

    [Header("Broadcast Events")] public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;
    public ObjectEventSO checkAvailableCardEvent;

    private bool shouldUpdateDiscardDeckUI = true;
    public int costChangeCounter; // 卡牌减费计数器（每打一张牌倒数一，为零时还原cost）
    public int costChangeAmount; // 减去的cost数量

    // 测试用
    private void Start()
    {
        InitializeDrawDeck();
        InitializeDiscardDeck();
        costChangeCounter = 0;
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
        drawCountEvent.RaiseEvent(drawDeck.Count, this);
    }

    private void InitializeDiscardDeck()
    {
        discardDeck.Clear();
        discardCountEvent.RaiseEvent(0, this);
    }

    // 监听sceneLoadCompleteEvent
    public void ResetCardDeck()
    {
        InitializeDrawDeck();
        InitializeDiscardDeck();
    }

    #region Draw Card

    [ContextMenu("TestDrawCard")]
    public void TestDrawCard()
    {
        CardRequest drawRequest = new(1, CardType.Any);
        DrawCard(drawRequest);
    }

    public void NewTurnDrawCard()
    {
        // TODO, 这个数量从player那里读取, 新建变量
        CardRequest newTurnDrawRequest = new(player.drawCountEachTurn, CardType.Any);
        DrawCard(newTurnDrawRequest);
    }

    public void DrawCard(object obj)
    {
        // Unpack request data from effect's delivery
        CardRequest drawRequest = (CardRequest)obj;
        int amount = drawRequest.amount;
        CardType cardType = drawRequest.cardType;

        for (int i = 0; i < amount; ++i)
        {
            if (handCardObjectList.Count >= 10) return; // Max hand limit is 10
            if (drawDeck.Count == 0)
            {
                RefillDrawDeckFromDiscard();
                ShuffDeck();
            }

            // No cards in both deck
            if (drawDeck.Count == 0) return;
            // Find the card with type in deck
            int index = 0;
            if (cardType != CardType.Any)
            {
                index = drawDeck.FindIndex(x => x.cardType == cardType);
                if (index == -1) continue; // No card of specified type found
            }

            CardDataSO drawedCardData = drawDeck[index];
            drawDeck.RemoveAt(index);
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

    #endregion

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; ++i)
        {
            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);
            currentCard.CheckCardAvailable();
            // Card draw animation
            currentCard.isAnimating = true;
            Sequence cardAnimationSequence = DOTween.Sequence();
            cardAnimationSequence
                .Append(currentCard.transform.DOScale(Vector3.one, animationTime).SetDelay(delay))
                .SetEase(Ease.InOutQuart)
                .Join(currentCard.transform.DOMove(cardTransform.position, animationTime)).SetEase(Ease.InOutQuart)
                .Join(currentCard.transform.DORotateQuaternion(cardTransform.rotation, animationTime))
                .SetEase(Ease.InOutQuart)
                .OnComplete(() => { currentCard.isAnimating = false; });
            // Set card order
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdateCardPositionRotation(cardTransform.position, cardTransform.rotation);
        }

        CheckAllCardsState();
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

    #region Discard Card

    public void DiscardAllCards()
    {
        discardCountEvent.RaiseEvent(discardDeck.Count + handCardObjectList.Count, this);
        shouldUpdateDiscardDeckUI = false;

        int remainingCoroutines = handCardObjectList.Count; // 计数器
        for (int i = handCardObjectList.Count - 1; i >= 0; --i)
        {
            float delay = (handCardObjectList.Count - i - 1) * 0.1f;
            DiscardCardWithDelay(handCardObjectList[i], delay, () =>
            {
                remainingCoroutines--;
                if (remainingCoroutines == 0)
                {
                    shouldUpdateDiscardDeckUI = true;
                }
            });
        }

        handCardObjectList.Clear();
    }

    private void DiscardCardWithDelay(object obj, float delay, System.Action onComplete)
    {
        StartCoroutine(DiscardCardCoroutine(obj, delay, onComplete));
    }

    private IEnumerator DiscardCardCoroutine(object obj, float delay, System.Action onComplete)
    {
        yield return new WaitForSeconds(delay);
        DiscardCard(obj); // 调用原始的 DiscardCard 方法
        onComplete?.Invoke();
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
        if (shouldUpdateDiscardDeckUI) discardCountEvent.RaiseEvent(discardDeck.Count, this);
        SetCardLayout(0);
    }

    #endregion

    public void CheckAllCardsState()
    {
        foreach (var card in handCardObjectList)
        {
            card.CheckCardAvailable();
        }

        // 检查手上有没有能出的牌
        foreach (var card in handCardObjectList)
        {
            if (card.isAvailable)
            {
                checkAvailableCardEvent.RaiseEvent(true, this);
                return;
            }
        }

        checkAvailableCardEvent.RaiseEvent(false, this);
    }

    #region Change Card Cost

    // 更新手牌花费
    // 持续更新到之后打出'sustainedNumber'张牌
    // public void UpdateCostChangeCounter()
    // {
    //     if (costChangeCounter > 0)
    //     {
    //         costChangeCounter--;
    //         if (costChangeCounter == 0)
    //         {
    //             UpdateHandCardsCost(true); // Reset
    //         }
    //     }
    // }

    // public void UpdateHandCardsCost(bool reset, int costChange = -1, int validTimes = 1)
    // {
    //     if (!reset) // 刷新持续事件和减少费用
    //     {
    //         costChangeAmount = costChange;
    //         costChangeCounter = validTimes;
    //     }else
    //     {
    //         costChangeAmount = 0;
    //         costChangeCounter = 0;
    //     }
    //     foreach (var card in handCardObjectList)
    //     {
    //         if (reset)
    //         {
    //             card.ResetCardCost();
    //             continue;
    //         }
    //         card.UpdateCardCost(costChangeAmount);
    //     }
    // }

    public void ChangeHandCardsCost(int costChange, int cardNumbers)
    {
        int cardsNeedToChange = cardNumbers;
        // randomly choose a card in hand and change its cost
        while (cardsNeedToChange > 0)
        {
            var validCards = handCardObjectList.FindAll(card => card.cardCost > 0);
            if (validCards.Count == 0) return; // 如果没有可减费的牌，就返回
            Card card = validCards[Random.Range(0, validCards.Count)];
            card.UpdateCardCost(costChange);
            cardsNeedToChange--;
        }
    }

    #endregion

    public List<CardDeckEntry> GetDrawDeck()
    {
        Dictionary<CardDataSO, int> drawDeckDict = new();
        foreach (var card in drawDeck)
        {
            if (drawDeckDict.ContainsKey(card))
            {
                drawDeckDict[card]++;
            }
            else
            {
                drawDeckDict.Add(card, 1);
            }
        }

        List<CardDeckEntry> drawCardList = new();
        foreach (var item in drawDeckDict)
        {
            drawCardList.Add(new CardDeckEntry(item.Key, item.Value));
        }

        return drawCardList;
    }

    public List<CardDeckEntry> GetDiscardDeck()
    {
        Dictionary<CardDataSO, int> discardDeckDict = new();
        foreach (var card in discardDeck)
        {
            if (discardDeckDict.ContainsKey(card))
            {
                discardDeckDict[card]++;
            }
            else
            {
                discardDeckDict.Add(card, 1);
            }
        }

        List<CardDeckEntry> discardCardList = new();
        foreach (var item in discardDeckDict)
        {
            discardCardList.Add(new CardDeckEntry(item.Key, item.Value));
        }

        return discardCardList;
    }
}