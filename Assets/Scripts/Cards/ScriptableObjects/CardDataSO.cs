using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    [TextArea]
    public string cardDescription;
    public Sprite cardSprite;
    public CardType cardType;
    public int cost;
    public int canExecuteTimes;
   
    public List<Effect> effects;
}