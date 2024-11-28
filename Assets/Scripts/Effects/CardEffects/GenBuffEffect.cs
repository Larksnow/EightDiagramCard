using UnityEngine;

// 艮卦阳/阴符buff效果
[CreateAssetMenu(fileName = "GenBuffEffect", menuName = "Card Effects/Gen Buff Effect")]
public class GenBuffEffect : Effect
{
    public DiagramDataSO genData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        genData.yinBuff = true;
        genData.yangBuff = true;
    }
}