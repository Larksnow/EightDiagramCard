using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    private List<CardDataSO> drawDeck = new();
    private List<CardDataSO> discardDeck = new();
    private List<Card> handCardObjectList = new();

    // 测试用
    private void Start()
    {
        InitializeDrawDeck();
    }

    public void InitializeDrawDeck()
    {
        drawDeck.Clear();
        // Add all cards in player's deck to draw deck
        foreach (var item in cardManager.playerHoldDeck.CardDeckEntryList) 
        {
            for(int i = 0; i < item.amount; ++i)
            { 
                drawDeck.Add(item.cardData);
            }
        }

        //TODO:洗牌/更新抽牌堆、弃牌堆数字
    }

    [ContextMenu("TestDrawCard")]
    public void TestDrawCard(){
        DrawCard(1);
    }
    private void DrawCard(int amount){
        for (int i = 0; i < amount; ++i)
        {
            if(drawDeck.Count == 0)
            {
            //TODO:洗牌/更新抽牌堆、弃牌堆数字
            }
            CardDataSO drawedCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            var card = cardManager.GetCardFromPool().GetComponent<Card>();
            // Initialize this drawed card
            card.Init(drawedCardData);
            handCardObjectList.Add(card);
        }
    }
}
