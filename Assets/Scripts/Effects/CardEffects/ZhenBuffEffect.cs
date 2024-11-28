using UnityEngine;

// 震卦阳/阴符buff效果
[CreateAssetMenu(fileName = "ZhenBuffEffect", menuName = "Card Effects/Zhen Buff Effect")]
public class ZhenBuffEffect : Effect
{
    public DiagramDataSO zhenData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        zhenData.yinBuff = true;
        zhenData.yangBuff = true;
    }
}