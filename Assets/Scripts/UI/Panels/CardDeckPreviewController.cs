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
    public float dimmerAlpha = 0.65f;

    private FadeInOutHandler fadeInOutHandler;
    private List<CardDeckEntry> cardList;
    private bool isDisplaying = false;

    private void Awake()
    {
        fadeInOutHandler = GetComponent<FadeInOutHandler>();
        panelBackground.enabled = false; // 隐藏背景
    }

    public void ToggleCardListPanel(CardListType cardListType)
    {
        isDisplaying = !isDisplaying;
        GameObject selectedDeck = null;
        if (isDisplaying)
        {
            switch (cardListType)
            {
                case CardListType.PlayerHold:
                    cardList = cardManager.playerHoldDeck.CardDeckEntryList;
                    selectedDeck = playerHoldDeckUI;
                    break;
                case CardListType.DrawDeck:
                    cardList = cardDeck.GetDrawDeck();
                    selectedDeck = drawDeckUI;
                    break;
                case CardListType.DiscardDeck:
                    cardList = cardDeck.GetDiscardDeck();
                    selectedDeck = discardDeckUI;
                    break;
            }

            panelBackground.enabled = true;

            // 背景变暗
            backgroundDimmer.SetActive(true);
            backgroundDimmer.GetComponent<FadeInOutHandler>().FadeIn(null, dimmerAlpha);

            // 将卡牌条目加入列表面板
            SetCardList();
            fadeInOutHandler.FadeIn();

            // 暂停游戏
            pauseManager.PauseGame();
        }
        else
        {
            // 淡出面板
            fadeInOutHandler.FadeOut(() =>
            {
                ClearCards();
                panelBackground.enabled = false;
                pauseManager.ResumeGame();
            });
            backgroundDimmer.GetComponent<FadeInOutHandler>().FadeOut(() => { backgroundDimmer.SetActive(false); });
        }
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