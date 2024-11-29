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
    public Color color; // 对应卦象的颜色（除BaseYang和BaseYin以外）
    public int cost;
    // public int canExecuteTimes;
   
    public List<Effect> effects;
}