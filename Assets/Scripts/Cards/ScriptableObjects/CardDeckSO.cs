using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDeck", menuName = "Card/CardDeck")]
public class CardDeckSO : ScriptableObject
{
    public List<CardDeckEntry> CardDeckEntryList;
    public void AddCard(CardDataSO cardData, int amount)
    {
        for (int i = 0; i < CardDeckEntryList.Count; i++)
        {
            if (CardDeckEntryList[i].cardData == cardData)
            {
                var entry = CardDeckEntryList[i];
                entry.amount += amount;
                CardDeckEntryList[i] = entry;
                return;
            }
        }
    }

    [System.Serializable]
    public struct CardDeckEntry
    {
        public CardDataSO cardData;
        public int amount;
    }
}