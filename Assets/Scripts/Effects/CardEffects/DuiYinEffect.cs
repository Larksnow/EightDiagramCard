using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "DuiYinEffect", menuName = "Card Effects/Dui Yin Effect")]
public class DuiYinEffect : CardEffect
{
    public CardDeck cardDeck;
    public int numOfReduced = 1;

    public override void Execute(DiagramDataSO triggeredDiagram)
    {
        if (cardDeck == null) cardDeck = GameObject.Find("CardDeck").GetComponent<CardDeck>();
        cardDeck.UpdateCardCost(false, -value, numOfReduced);
        Debug.Log("下一张牌费用减少1" + "Time:" + Time.time);
    }
}