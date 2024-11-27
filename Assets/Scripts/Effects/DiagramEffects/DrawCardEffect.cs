using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Diagram Effects/Draw Card Effect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;

    public override void Execute(CharacterBase target, DiagramDataSO diagramData)
    {
        drawCardEvent.RaiseEvent(value, this);
    }
}
