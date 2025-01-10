using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Diagram Effects/Draw Special Card Effect")]
public class DrawSpecialCardEffect : DrawCardEffect
{
    public CardType type;
    public override void Execute(CharacterBase target)
    {
        CardRequest cardRequest = new CardRequest(tempValue, type);
        drawCardEvent.RaiseEvent(cardRequest, this);
    }

    public override string GetFormattedDescription()
    {
        return description.Replace("{value}", tempValue.ToString());
    }
}
