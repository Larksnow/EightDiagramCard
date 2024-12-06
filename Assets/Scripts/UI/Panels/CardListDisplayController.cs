using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardListDisplayController : MonoBehaviour
{
    public GameObject cardEntryPrefab;
    public Transform scrollContent;    // 滚动条目的父对象
    public Image backgroundImage;

    private CardDeck cardDeck;
    private CardManager cardManager;
    private FadeInOutHander fadeInOutHander;
    private List<CardDeckEntry> cardList;
    private GameObject drawDeckUI;
    private GameObject discardDeckUI;
    private GameObject playerHoldDeckUI;
    private PauseManager pauseManager;
    private bool isDisplaying = false;
    private float entryHeight;

    private void Awake()
    {
        cardDeck = GameObject.Find("CardDeck").GetComponent<CardDeck>();
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        fadeInOutHander = GetComponent<FadeInOutHander>();
        drawDeckUI = GameObject.Find("DrawDeckUI");
        discardDeckUI = GameObject.Find("DiscardDeckUI");
        playerHoldDeckUI = GameObject.Find("PlayerHoldDeckUI");
        pauseManager = PauseManager.Instance;
        backgroundImage.enabled = false;

        // 获取卡牌条目的高度（用于计算滚动区域）
        entryHeight = cardEntryPrefab.GetComponent<RectTransform>().rect.height;
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

            // 将卡牌条目加入列表面板
            SetCardList();
            fadeInOutHander.FadeIn();

            // 暂停游戏
            pauseManager.PauseGame(new List<Button> { selectedDeck.GetComponent<Button>() });
        }
        else
        {
            // 恢复游戏
            fadeInOutHander.FadeOut(() =>
            {
                backgroundImage.enabled = false;
                Clear();
            });
            pauseManager.ResumeGame();
        }
    }


    private void SetCardList()
    {
        backgroundImage.enabled = true;
        if (cardList != null)
        {
            foreach (CardDeckEntry cardEntry in cardList)
            {
                GameObject cardEntryObj = Instantiate(cardEntryPrefab, scrollContent);
                cardEntryObj.GetComponent<TextMeshProUGUI>().text = $"{cardEntry.cardData.cardName}: {cardEntry.amount} 张";
            }
        }
    }

    private void Clear()
    {
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }
    }
}