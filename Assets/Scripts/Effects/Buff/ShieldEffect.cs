using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldEffect", menuName = "Buff/Shield Effect")]
public class ShieldEffect : Effect
{
    // 化劲(护甲，可以直接抵消敌人的伤害, 下回合开始多余的化劲会清空)
    int number = 3; //每次该效果施加的层数
    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        Debug.Log($"Passing {number} to {target}");
        target.UpdateShield(number);
    }
}
