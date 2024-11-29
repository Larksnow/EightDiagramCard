using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldEffect", menuName = "Buff/Shield Effect")]
public class ShieldEffect : Effect
{
    // 化劲(护甲，可以直接抵消敌人的伤害, 下回合开始多余的化劲会清空)
    public int buffNumber = 1;  // 有buff时的额外层数
    public DiagramDataSO kanData;   // 专属于坎卦

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        int shieldAmount = value;
        if (kanData.yinBuff || kanData.yangBuff) 
            shieldAmount += buffNumber;
        target.UpdateShield(shieldAmount);
    }
}
