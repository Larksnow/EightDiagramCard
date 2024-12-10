using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Diagram Effects/Decrease Card Cost Effect")]
public class DecreaseCardCostEffect : DiagramEffect
{
    [SerializeField] private CardDeck cardDeck;
    public int costReduction = 1;
    public override void Execute(CharacterBase target)
    {
        if (cardDeck == null) cardDeck = GameObject.Find("CardDeck").GetComponent<CardDeck>();
        cardDeck.ChangeHandCardsCost(-costReduction, tempValue);
    }

    public override string GetFormattedDescription()
    {
        return description.Replace("{value}", tempValue.ToString());
    }
}
