using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/QianYangData")]
public class QianYangDataSO : CardDataSO
{
    public CardManager cardManager;
  
    public string GetUpdatedCardDescription()
    {
        if (cardManager == null) cardManager = FindObjectOfType<CardManager>();
        CardDataSO previousCard = cardManager.previousCard;
        string copiedDescripion = previousCard == null ? string.Empty : $"\n(当前: {previousCard.cardDescription})";
        return cardDescription + copiedDescripion;
    }
}