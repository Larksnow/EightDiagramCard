using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Diagram Effects/Draw Card Effect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;

    public override void Execute(CharacterBase target)
    {
        drawCardEvent.RaiseEvent(value, this);
    }
    public override void Execute(DiagramDataSO diagramData)
    {
        throw new System.NotImplementedException();
    }
}
