using UnityEngine;

[CreateAssetMenu(fileName = "EnhenceCopyDiagramEffect", menuName = "Card Effects/Enhence Copy Diagram Effect")]
public class EnhenceCopyDiagramEffect : CardEffect
{
    public CopyDiagramEffect copyDiagramEffect;

    public override void Execute(DiagramDataSO triggeredDiagram)
    {
        copyDiagramEffect.enhancedCopyDiagram.Add(triggeredDiagram);
        Debug.Log($"{triggeredDiagram} was enhanced if copied");
    }
}