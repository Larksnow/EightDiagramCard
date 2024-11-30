using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Diagram Effects/Draw Card Effect")]
public class DrawCardEffect : Effect
{
    public ObjectEventSO drawCardEvent;
    public DiagramDataSO xunData;
    // This dict used to store extra cards to draw (added through cards effect)
    public Dictionary<CardType, int> specialCardsToDraw;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        // deal with any type drawing first
        int normalCards = xunData.basicValue + xunData.buffedValue + xunData.tempValue;
        CardRequest request = new CardRequest(normalCards, CardType.Any);
        drawCardEvent.RaiseEvent(request, this);
        // deal with specific type drawing
        if (specialCardsToDraw != null)
        {
            foreach (var pair in specialCardsToDraw)
            {
                CardType wantedType = pair.Key;
                int count = pair.Value;

                CardRequest specialCardRequest = new CardRequest(count, wantedType);
                drawCardEvent.RaiseEvent(specialCardRequest, this);
            }
            specialCardsToDraw.Clear();
        }
    }
}
