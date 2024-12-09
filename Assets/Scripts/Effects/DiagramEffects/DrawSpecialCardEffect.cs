using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpecialCardEffect : DrawCardEffect
{
    public int number;
    public CardType type;
    public override void Execute(CharacterBase target)
    {
        CardRequest cardRequest = new CardRequest(number, type);
        drawCardEvent.RaiseEvent(cardRequest, this);
    }
}
