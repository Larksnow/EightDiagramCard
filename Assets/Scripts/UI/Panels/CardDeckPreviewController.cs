using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardDeckPreviewController : MonoBehaviour
{
    public float fadeDuration = 0.4f;
    public float moveDurationOnSort = 0.5f;
    public Ease easeTypeOnSort;

    [Header("Card Deck UI")] public GameObject drawDeckUI;
    public GameObject discardDeckUI;
    public GameObject playerHoldDeckUI;

    [Header("Managers")] public PoolTool pool;
    public CardDeck cardDeck;
    public CardManager cardManager;
    public PauseManager pauseManager;

    [Header("UI Components")] public Transform scrollContent;
    public GameObject backgroundDimmer;
    public GameObject returnButton;
    public GameObject sortControlBar;
    public float dimmerAlpha = 0.65f;

    private List<CardDeckEntry> cardList;
    private bool isDisplaying; // 面板是否正在显示
    private List<CardPreview> cardPreviews = new();
    private int cardAmount = 0;

    private void Awake()
    {
        backgroundDimmer.SetActive(false);
        returnButton.SetActive(false);
        sortControlBar.SetActive(false);
    }

    private void Start()
    {
        // 注册事件监听
        sortControlBar.GetComponent<SortControlBar>().OnSortChanged += OnSortChanged;
    }

    # region Event Listening

    // 打开面板
    public void OpenCardPreview(GameObject selected)
    {
        isDisplaying = true;
        if (selected == playerHoldDeckUI)
        {
            cardList = cardManager.playerHoldDeck.CardDeckEntryList;
            sortControlBar.SetActive(true); // 只有打开玩家手牌时才显示排序控制栏
        }
        else if (selected == drawDeckUI)
        {
            cardList = cardDeck.GetDrawDeck();
        }
        else if (selected == discardDeckUI)
        {
            cardList = cardDeck.GetDiscardDeck();
        }

        // 启用面板组件
        backgroundDimmer.SetActive(true);
        returnButton.SetActive(true);

        // 将卡牌条目加入列表面板
        SetCardList();

        // 淡入面板
        FadeInPanel();

        // 暂停游戏
        pauseManager.PauseGame(new List<Button> { returnButton.GetComponent<Button>() });
    }

    // 关闭面板
    // 直接监听ReturnButton上内置Button组件的OnClick事件, 而不是自定义的OnClickEvent事件
    public void CloseCardPreview()
    {
        if (!isDisplaying) return;
        isDisplaying = false;

        // 淡出面板
        FadeOutPanel();
    }

    public void OnSortChanged(SortControlBar.SortType sortType, bool isAscending)
    {
        switch (sortType)
        {
            case SortControlBar.SortType.AcquisitionOrder:
                cardPreviews.Sort((a, b) => CompareByAcquisitionOrder(a, b, isAscending));
                break;
            case SortControlBar.SortType.ManaCost:
                cardPreviews.Sort((a, b) => CompareByManaCost(a, b, isAscending));
                break;
        }


        // 重新排列卡牌
        // 添加缓动效果
        Sequence sortSequence = DOTween.Sequence();
        for (int i = 0; i < cardAmount; i++)
        {
            RectTransform targetTransform = scrollContent.GetChild(i).GetComponent<RectTransform>();
            sortSequence.Join(
                cardPreviews[i].gameObject.GetComponent<RectTransform>()
                    .DOLocalMove(targetTransform.localPosition, moveDurationOnSort).SetEase(easeTypeOnSort));
        }

        sortSequence.onComplete += () =>
        {
            for (int i = 0; i < cardAmount; i++)
            {
                cardPreviews[i].transform.SetSiblingIndex(i);
            }
        };
    }

    #endregion


    private void SetCardList()
    {
        // 默认为获得顺序升序排列（后获得的卡牌在下方）
        cardAmount = 0;
        cardPreviews.Clear();
        if (cardList != null)
        {
            foreach (CardDeckEntry cardEntry in cardList)
            {
                for (int i = 0; i < cardEntry.amount; i++)
                {
                    // 从对象池中取出CardPreview,并初始化
                    GameObject cardPreviewObj = pool.GetObjectFromPool("CardPreview");
                    cardPreviewObj.transform.SetParent(scrollContent, false);
                    CardDataSO cardData = cardEntry.cardData;
                    CardPreview cardPreview = cardPreviewObj.GetComponent<CardPreview>();
                    cardPreview.Init(cardData, cardAmount++);
                    cardPreviews.Add(cardPreview);
                }
            }
        }
    }

    private void ClearCards()
    {
        foreach (Transform child in scrollContent)
        {
            pool.ReleaseObjectToPool("CardPreview", child.gameObject);
        }

        cardAmount = 0;
    }

    private int CompareByAcquisitionOrder(CardPreview a, CardPreview b, bool isAscending)
    {
        return isAscending
            ? a.AcquisitionIndex.CompareTo(b.AcquisitionIndex)
            : b.AcquisitionIndex.CompareTo(a.AcquisitionIndex);
    }

    private int CompareByManaCost(CardPreview a, CardPreview b, bool isAscending)
    {
        return isAscending
            ? a.cardData.cost.CompareTo(b.cardData.cost)
            : b.cardData.cost.CompareTo(a.cardData.cost);
    }

    #region FadeInOut

    // 由于背景Image要固定隐藏（透明度为0），不能使用FadeInOutHandler
    private void FadeInPanel()
    {
        FadeInOutHandler.FadeObjectIn(backgroundDimmer, null, dimmerAlpha, fadeDuration);
        FadeInOutHandler.FadeObjectIn(returnButton, null, 1f, fadeDuration);
        if (sortControlBar.activeSelf)
            FadeInOutHandler.FadeObjectIn(sortControlBar, null, 1f, fadeDuration);
        foreach (Transform card in scrollContent)
        {
            if (card.gameObject.activeSelf)
            {
                FadeInOutHandler.FadeObjectIn(card.gameObject, null, 1f, fadeDuration);
            }
        }
    }

    private void FadeOutPanel()
    {
        FadeInOutHandler.FadeObjectOut(backgroundDimmer, () => { backgroundDimmer.SetActive(false); }, fadeDuration);
        FadeInOutHandler.FadeObjectOut(returnButton, () => { returnButton.SetActive(false); }, fadeDuration);
        if (sortControlBar.activeSelf)
            FadeInOutHandler.FadeObjectOut(sortControlBar, () => { sortControlBar.SetActive(false); }, fadeDuration);
        for (int i = 0; i < cardAmount; i++)
        {
            Transform card = scrollContent.GetChild(i);
            if (card.gameObject.activeSelf)
            {
                if (i == cardAmount - 1)
                {
                    FadeInOutHandler.FadeObjectOut(card.gameObject, () =>
                    {
                        ClearCards();
                        pauseManager.ResumeGame();
                    }, fadeDuration);
                }
                else
                {
                    FadeInOutHandler.FadeObjectOut(card.gameObject, null, fadeDuration);
                }
            }
        }
    }

    #endregion

    public int GetCardAmount()
    {
        return cardAmount;
    }
}