using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectCardPanel : MonoBehaviour
{
    public GameObject title, skipButton, cardPos1Obj, cardPos2Obj, cardPos3Obj;
    public List<GameObject> cardPosObjs;
    public CardManager cardManager;
    public PauseManager pauseManager;
    public FadeInOutHander fadeInOutHander;

    private List<CardDataSO> cardsForSelection = new();

    [Header("Broadcast Events")]
    // TODO: 选择卡牌/跳过后进入下一关
    public ObjectEventSO nextLevelEvent;

    private void Awake()
    {
        cardPosObjs.Add(cardPos1Obj);
        cardPosObjs.Add(cardPos2Obj);
        cardPosObjs.Add(cardPos3Obj);
        pauseManager = PauseManager.Instance;
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
    }

    private void OnEnable()
    {
        SetCardsForAward();
        fadeInOutHander.FadeIn();
        pauseManager.PauseGame();
    }

    private void OnDisable()
    {
        foreach (var cardObj in cardPosObjs)
        {
            GameObject card = cardObj.transform.GetChild(0).gameObject;
            cardManager.DiscardCard(card);
        }
        pauseManager.UnpauseGame();
    }

    #region Event Listening
    public void OnClick(object obj)
    {
        PointerEventData pointerEventData = (PointerEventData)obj;
        GameObject selected = pointerEventData.pointerPress;
        if (selected.transform.IsChildOf(transform))
        {
            if (selected == skipButton)
            {
                nextLevelEvent.RaiseEvent(null, this);
            }
            else
            {
                // 选择一张卡牌加入手牌
                for (int i = 0; i < cardPosObjs.Count; i++)
                {
                    if (selected == cardPosObjs[i])
                    {
                        cardManager.playerHoldDeck.AddCard(cardsForSelection[i], 1);
                        //TODO: 播放卡牌动画
                    }
                }
                nextLevelEvent.RaiseEvent(null, this);
                StartCoroutine(FadeOutCoroutine());
            }
        }
    }
    #endregion

    private IEnumerator FadeOutCoroutine()
    {
        fadeInOutHander.FadeOut();
        yield return new WaitForSeconds(fadeInOutHander.fadeDuration);
        gameObject.SetActive(false);
    }

    // 初始化卡牌选择面板
    private void SetCardsForAward()
    {
        List<int> cardIndexes = new();
        if (cardPosObjs.Count > cardManager.cardDataList.Count)
            Debug.LogWarning("Not enough cards for award");

        for (int i = 0; i < cardPosObjs.Count; i++)
        {
            int index;
            // 不能重复选择相同的卡牌
            do
            {
                index = Random.Range(0, cardManager.cardDataList.Count);
            } while (cardIndexes.Contains(index));
            cardIndexes.Add(index);

            CardDataSO cardData = cardManager.cardDataList[index];
            GameObject cardObj = cardManager.GetCardFromPool();
            Card card = cardObj.GetComponent<Card>();
            card.Init(cardData);
            cardObj.transform.SetParent(cardPosObjs[i].transform, false);
            cardObj.transform.localScale = Vector3.one;
            cardsForSelection.Add(card.cardData);
        }
    }
}
