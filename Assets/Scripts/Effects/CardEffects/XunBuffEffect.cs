using UnityEngine;

// 巽卦阳/阴符buff效果
[CreateAssetMenu(fileName = "XunBuffEffect", menuName = "Card Effects/Xun Buff Effect")]
public class XunBuffEffect : Effect
{
    public DiagramDataSO xunData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (cardType == CardType.Yin)
        {
            xunData.yinBuff = true;
        }
        else
        {
            xunData.yangBuff = true;
        }
    }
}