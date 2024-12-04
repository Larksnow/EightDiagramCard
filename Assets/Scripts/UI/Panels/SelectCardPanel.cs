using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectCardPanel : MonoBehaviour, ButtonClickHandler
{
    public GameObject skipButton, cardPos1Obj, cardPos2Obj, cardPos3Obj;
    public List<GameObject> cardPosObjs;
    public CardManager cardManager;

    private PauseManager pauseManager;
    private FadeInOutHander fadeInOutHander;
    private List<CardDataSO> cardsForSelection = new();
    private List<GameObject> excludeFromPauseList = new();

    [Header("Broadcast Events")]
    public ObjectEventSO nextLevelEvent;
    public ObjectEventSO addCardToHoldDeckEvent;

    private void Awake()
    {
        pauseManager = PauseManager.Instance;
        fadeInOutHander = GetComponent<FadeInOutHander>();
        cardPosObjs.Add(cardPos1Obj);
        cardPosObjs.Add(cardPos2Obj);
        cardPosObjs.Add(cardPos3Obj);

        // 将可点击组件加入剔除暂停列表，不受暂停影响
        excludeFromPauseList.Add(skipButton);
        excludeFromPauseList.Add(cardPos1Obj);
        excludeFromPauseList.Add(cardPos2Obj);
        excludeFromPauseList.Add(cardPos3Obj);
    }

    private void OnEnable()
    {
        Clear();
        SetCardsForAward();
        fadeInOutHander.FadeIn();

        pauseManager.PauseGame(excludeFromPauseList);
    }

    private void OnDisable()
    {
        pauseManager.ResumeGame();
    }

    #region Event Listening
    public void OnClick(object obj)
    {
        PointerEventData pointerEventData = (PointerEventData)obj;
        GameObject selected = pointerEventData.pointerPress;
        if (selected.transform.IsChildOf(transform))
        {
            if (selected != skipButton)
            {
                // 选择一张卡牌加入手牌
                for (int i = 0; i < cardPosObjs.Count; i++)
                {
                    if (selected == cardPosObjs[i])
                    {
                        cardManager.AddCardToPlayerHoldDeck(cardsForSelection[i], 1);
                        // 通知ui更新动画
                        addCardToHoldDeckEvent.RaiseEvent(selected.transform.GetChild(0).gameObject, this);
                    }
                }
            }
            selected.GetComponent<Button>().FadeOutAfterClick(fadeInOutHander,
            () => { gameObject.SetActive(false); });
            nextLevelEvent.RaiseEvent(null, this);
        }
    }
    #endregion

    // 清空选择面板处上次留下的卡牌
    private void Clear()
    {
        foreach (var cardObj in cardPosObjs)
        {
            if (cardObj.transform.childCount > 0)
            {
                foreach (Transform child in cardObj.transform)
                {
                    Debug.Log(child.gameObject.name);
                    child.gameObject.GetComponent<CardDragHandler>().enabled = true;   // 丢弃前恢复拖拽组件
                    cardManager.DiscardCard(child.gameObject);
                }
            }
        }
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

            // 从卡池中取出一张卡牌并初始化
            CardDataSO cardData = cardManager.cardDataList[index];
            GameObject cardObj = cardManager.GetCardFromPool();
            Card card = cardObj.GetComponent<Card>();
            card.Init(cardData, false);
            cardObj.transform.SetParent(cardPosObjs[i].transform, false);
            cardObj.transform.localPosition = Vector3.zero;
            cardObj.transform.localScale = Vector3.one;
            CardDragHandler cardDragHandler = cardObj.GetComponent<CardDragHandler>();// 禁用拖拽
            cardDragHandler.enabled = false;
            cardsForSelection.Add(card.cardData);   // 添加到可选列表
        }
    }
}
