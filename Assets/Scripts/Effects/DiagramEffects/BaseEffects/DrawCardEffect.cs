using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Diagram Effects/Draw Card Effect")]
public class DrawCardEffect : DiagramEffect
{
    public ObjectEventSO drawCardEvent;
    public override void Execute(CharacterBase target)
    {
        // deal with any type drawing first
        CardRequest request = new CardRequest(value, CardType.Any);
        drawCardEvent.RaiseEvent(request, this);
    }
}
