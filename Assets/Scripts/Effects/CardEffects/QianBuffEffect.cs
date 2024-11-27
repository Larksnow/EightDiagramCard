using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "QianBuffEffect", menuName = "Card Effects/Qian Buff Effect")]
public class QianBuffEffect : Effect
{
    public override void Execute(CharacterBase target, DiagramDataSO diagramData)
    {
        if (diagramData == null) return;    // 还没有组成完整的卦象
        if (diagramData.diagramType != DiagramType.Qian) return; // 不能对非乾卦执行
        diagramData.yangBuff = true;
        diagramData.yinBuff = true;
        Debug.Log("QianBuffEffect executed!");
    }
}