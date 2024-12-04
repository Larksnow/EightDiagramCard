using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardListPanelController : MonoBehaviour
{
    public GameObject cardEntryPrefab;
    public Vector3 beginPos = new(0, 0, 0);
    public float spacing = 70;

    public CardDeck cardDeck;
    public CardManager cardManager;
    public FadeInOutHander fadeInOutHander;
    private List<CardDeckEntry> cardList;
    public Transform scrollContent;    // 滚动的卡牌条目内容
    public GameObject drawDeckUI;
    public GameObject discardDeckUI;
    public GameObject playerHoldDeckUI;
    public PauseManager pauseManager;
    private bool isDisplaying = false;
    private Vector3 currentPos;
    private float entryHeight;

    private void Awake()
    {
        fadeInOutHander = GetComponent<FadeInOutHander>();
        pauseManager = PauseManager.Instance;
        currentPos = beginPos;

        // 获取卡牌条目的高度（用于计算滚动区域）
        entryHeight = cardEntryPrefab.GetComponent<RectTransform>().rect.height + spacing;
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
            SetAllActive(true);
            SetCardList();
            fadeInOutHander.FadeIn();

            // 暂停游戏
            pauseManager.PauseGame(new List<GameObject> { selectedDeck });
        }
        else
        {
            // 恢复游戏
            fadeInOutHander.FadeOut();
            pauseManager.ResumeGame();
        }
    }


    private void SetCardList()
    {
        if (cardList != null)
        {
            // 清空之前的卡牌条目
            foreach (Transform child in scrollContent)
            {
                Destroy(child.gameObject);
            }

            currentPos = beginPos;

            // 计算滚动区域的高度
            float contentHeight = cardList.Count * entryHeight;
            RectTransform contentRect = scrollContent.GetComponent<RectTransform>();
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);

            foreach (CardDeckEntry cardEntry in cardList)
            {
                GameObject cardEntryObj = Instantiate(cardEntryPrefab, scrollContent);
                cardEntryObj.GetComponent<TextMeshProUGUI>().text = $"{cardEntry.cardData.cardName}: {cardEntry.amount}张";
                cardEntryObj.transform.localPosition = currentPos;
                currentPos.y -= spacing;
            }
        }
    }

    private void SetAllActive(bool isActive)
    {
        foreach (Transform child in transform) { child.gameObject.SetActive(isActive); }
    }

    public bool IsDisplaying()
    {
        return isDisplaying;
    }
}
