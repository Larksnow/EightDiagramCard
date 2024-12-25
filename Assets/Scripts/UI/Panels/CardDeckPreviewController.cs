using System.Collections.Generic;
using UnityEngine;

public class CardDeckPreviewController : MonoBehaviour
{
    public float fadeDuration = 0.4f;
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
    public float dimmerAlpha = 0.65f;

    private List<CardDeckEntry> cardList;
    private bool isDisplaying;
    private int cardAmount = 0;

    // 打开面板
    public void OpenCardPreview(GameObject selected)
    {
        isDisplaying = true;
        if (selected == playerHoldDeckUI)
        {
            cardList = cardManager.playerHoldDeck.CardDeckEntryList;
        }
        else if (selected == drawDeckUI)
        {
            cardList = cardDeck.GetDrawDeck();
        }
        else if (selected == discardDeckUI)
        {
            cardList = cardDeck.GetDiscardDeck();
        }

        // 背景变暗
        backgroundDimmer.SetActive(true);

        // 启用返回按钮
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
                    cardAmount++;
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


    // 由于背景Image要固定隐藏（透明度为0），不能使用FadeInOutHandler
    private void FadeInPanel()
    {
        FadeInOutHandler.FadeObjectIn(backgroundDimmer, null, dimmerAlpha, fadeDuration);
        FadeInOutHandler.FadeObjectIn(returnButton,null, 1f, fadeDuration);
        foreach (Transform card in scrollContent)
        {
            FadeInOutHandler.FadeObjectIn(card.gameObject, null, 1f, fadeDuration);
        }
    }

    private void FadeOutPanel()
    {
        FadeInOutHandler.FadeObjectOut(backgroundDimmer, () => { backgroundDimmer.SetActive(false); }, fadeDuration);
        FadeInOutHandler.FadeObjectOut(returnButton, () => { returnButton.SetActive(false); }, fadeDuration);
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Transform card = scrollContent.GetChild(i);
            if (i == scrollContent.childCount - 1)
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

    public int GetCardAmount()
    {
        return cardAmount;
    }
}