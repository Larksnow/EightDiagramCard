using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CardDeck", menuName = "Card/CardDeck")]
public class CardDeckSO : ScriptableObject
{
    public List<CardDeckEntry> CardDeckEntryList;
}

[System.Serializable]
public struct CardDeckEntry
{
    public CardDataSO cardData;
    public int amount;
}