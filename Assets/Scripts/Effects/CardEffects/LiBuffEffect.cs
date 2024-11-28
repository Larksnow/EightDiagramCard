using UnityEngine;

// 坎卦阳/阴符buff效果
[CreateAssetMenu(fileName = "LiBuffEffect", menuName = "Card Effects/Li Buff Effect")]
public class LiBuffEffect : Effect
{
    public DiagramDataSO liData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        liData.yinBuff = true;
        liData.yangBuff = true;
    }
}