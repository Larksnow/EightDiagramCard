using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "QianBuffEffect", menuName = "Card Effects/Qian Buff Effect")]
public class QianBuffEffect : Effect
{
    public DiagramDataSO qianData;
    public CopyDiagramEffect copyDiagramEffect;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        copyDiagramEffect.enhancedCopyTypes.Add(triggered.diagramType);
        qianData.yangBuff = true;
        qianData.yinBuff = true;
        Debug.Log($"{triggered.name} was enhanced with Qian Buff.");
    }
}