using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Diagram Effects/Draw Card Effect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        drawCardEvent.RaiseEvent(value, this);
    }

}
