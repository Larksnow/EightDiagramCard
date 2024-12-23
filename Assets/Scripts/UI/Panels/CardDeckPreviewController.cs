using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDeckPreviewController : MonoBehaviour
{
    [Header("Card Deck UI")] public GameObject drawDeckUI;
    public GameObject discardDeckUI;
    public GameObject playerHoldDeckUI;

    [Header("Managers")] public PoolTool pool;
    public CardDeck cardDeck;
    public CardManager cardManager;
    public PauseManager pauseManager;

    [Header("UI Components")] public Image panelBackground;
    public Transform scrollContent;
    public GameObject backgroundDimmer;
    public GameObject returnButton;
    public float dimmerAlpha = 0.65f;

    private FadeInOutHandler fadeInOutHandler;
    private List<CardDeckEntry> cardList;
    private bool isDisplaying;

    private void Awake()
    {
        fadeInOutHandler = GetComponent<FadeInOutHandler>();
        panelBackground.enabled = false; // 隐藏背景
    }

    // 打开面板
    public void OpenCardPreview(CardListType cardListType)
    {
        isDisplaying = true;
        switch (cardListType)
        {
            case CardListType.PlayerHold:
                cardList = cardManager.playerHoldDeck.CardDeckEntryList;
                break;
            case CardListType.DrawDeck:
                cardList = cardDeck.GetDrawDeck();
                break;
            case CardListType.DiscardDeck:
                cardList = cardDeck.GetDiscardDeck();
                break;
        }

        panelBackground.enabled = true;

        // 背景变暗
        backgroundDimmer.SetActive(true);
        FadeInOutHandler.FadeObjectIn(backgroundDimmer, null, dimmerAlpha);

        // 启用返回按钮
        returnButton.SetActive(true);
        FadeInOutHandler.FadeObjectIn(returnButton, null, 1f);

        // 将卡牌条目加入列表面板
        SetCardList();
        fadeInOutHandler.FadeIn();

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
        fadeInOutHandler.FadeOut(() =>
        {
            ClearCards();
            panelBackground.enabled = false;
            pauseManager.ResumeGame();
        });
        FadeInOutHandler.FadeObjectOut(backgroundDimmer, () => { backgroundDimmer.SetActive(false); });
        FadeInOutHandler.FadeObjectOut(returnButton, () => { returnButton.SetActive(false); });
    }


    private void SetCardList()
    {
        if (cardList != null)
        {
            foreach (CardDeckEntry cardEntry in cardList)
            {
                for (int i = 0; i < cardEntry.amount; i++)
                {
                    CardDataSO cardData = cardEntry.cardData;
                    GameObject cardPreviewObj = pool.GetObjectFromPool("CardPreview");
                    cardPreviewObj.transform.SetParent(scrollContent, false);
                    CardPreview cardPreview = cardPreviewObj.GetComponent<CardPreview>();
                    cardPreview.Init(cardData);
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
    }
}