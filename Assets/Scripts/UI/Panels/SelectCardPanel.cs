using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectCardPanel : FadablePanel, ButtonClickHandler
{
    public GameObject skipButton, cardPos1Obj, cardPos2Obj, cardPos3Obj;
    public List<GameObject> cardPosObjs;
    public CardManager cardManager;

    private List<CardDataSO> cardsForSelection = new();

    [Header("Broadcast Events")]
    public ObjectEventSO nextLevelEvent;
    public ObjectEventSO addCardToHoldDeckEvent;

    protected override void Awake()
    {
        base.Awake();
        cardPosObjs.Add(cardPos1Obj);
        cardPosObjs.Add(cardPos2Obj);
        cardPosObjs.Add(cardPos3Obj);
    }

    protected override void OnEnable()
    {
        ClearCards();
        SetCardsForAward();
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnClickSelected(GameObject selected)
    {
        base.OnClickSelected(selected);
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
        nextLevelEvent.RaiseEvent(null, this);
    }

    // 清空选择面板处上次留下的卡牌
    private void ClearCards()
    {
        foreach (var cardObj in cardPosObjs)
        {
            if (cardObj.transform.childCount > 0)
            {
                foreach (Transform child in cardObj.transform)
                {
                    Debug.Log(child.gameObject.name);
                    child.gameObject.GetComponent<CardDragHandler>().enabled = true;   // 丢弃前恢复拖拽和卡牌脚本
                    child.gameObject.GetComponent<Card>().enabled = true;
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
            cardObj.GetComponent<CardDragHandler>().enabled = false;// 禁用拖拽
            cardObj.GetComponent<Card>().enabled = false;          // 禁用卡牌脚本
            cardsForSelection.Add(card.cardData);   // 添加到可选列表
        }
    }
}
