using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "QianYinEffect", menuName = "Card Effects/Qian Yin Effect")]
public class QianYinEffect : Effect
{
    public DiagramDataSO qianData;
    public CopyDiagramEffect copyDiagramEffect;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        copyDiagramEffect.enhancedCopyDiagram.Add(triggered);
        Debug.Log($"{triggered.name} was enhanced with Qian Yin.");
    }
}