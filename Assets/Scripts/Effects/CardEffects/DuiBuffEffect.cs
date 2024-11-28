using UnityEngine;

// 兑卦阳/阴符buff效果
[CreateAssetMenu(fileName = "DuiBuffEffect", menuName = "Card Effects/Dui Buff Effect")]
public class DuiBuffEffect : Effect
{
    public DiagramDataSO duiData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (cardType == CardType.Yin)
        {
            duiData.yinBuff = true;
        }
        else
        {
            duiData.yangBuff = true;
        }
    }
}