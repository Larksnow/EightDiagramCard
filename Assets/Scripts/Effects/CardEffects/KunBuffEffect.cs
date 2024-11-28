using UnityEngine;

// 坤卦阳/阴符buff效果
[CreateAssetMenu(fileName = "KunBuffEffect", menuName = "Card Effects/Kun Buff Effect")]
public class KunBuffEffect : Effect
{
    public CardDeck cardDeck;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (cardDeck == null) cardDeck = GameObject.Find("CardDeck").GetComponent<CardDeck>();
        cardDeck.UpdateCardCost(false, -1, 1);
    }
}