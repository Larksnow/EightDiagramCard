using UnityEngine;

// 坎卦阳/阴符buff效果
[CreateAssetMenu(fileName = "KanBuffEffect", menuName = "Card Effects/Kan Buff Effect")]
public class KanBuffEffect : Effect
{
    public DiagramDataSO kanData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        kanData.yinBuff = true;
        kanData.yangBuff = true;
    }
}