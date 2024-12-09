using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardListDisplayController : MonoBehaviour
{
    public GameObject cardEntryPrefab;
    public Transform scrollContent; // 滚动条目的父对象
    public Image backgroundImage;

    private GameObject drawDeckUI;
    private GameObject discardDeckUI;
    private GameObject playerHoldDeckUI;
    private CardDeck cardDeck;
    private CardManager cardManager;
    private FadeInOutHandler fadeInOutHandler;
    private List<CardDeckEntry> cardList;
    private PauseManager pauseManager;
    private bool isDisplaying = false;

    private void Awake()
    {
        drawDeckUI = GameObject.Find("DrawDeckUI");
        discardDeckUI = GameObject.Find("DiscardDeckUI");
        playerHoldDeckUI = GameObject.Find("PlayerHoldDeckUI");
        cardDeck = GameObject.Find("CardDeck").GetComponent<CardDeck>();
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        fadeInOutHandler = GetComponent<FadeInOutHandler>();
        pauseManager = PauseManager.Instance;
        backgroundImage.enabled = false;
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
            fadeInOutHandler.FadeIn();

            // 暂停游戏
            pauseManager.PauseGame(new List<Button> { selectedDeck.GetComponent<Button>() });
        }
        else
        {
            // 恢复游戏
            fadeInOutHandler.FadeOut(() =>
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
                cardEntryObj.GetComponent<TextMeshProUGUI>().text =
                    $"{cardEntry.cardData.cardName}: {cardEntry.amount} 张";
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