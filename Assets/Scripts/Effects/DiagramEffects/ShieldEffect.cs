using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldEffect", menuName = "Diagram Effects/Shield Effect")]
public class ShieldEffect : Effect
{
    // 化劲(护甲，可以直接抵消敌人的伤害, 下回合开始多余的化劲会清空)
    public DiagramDataSO kanData;   // 专属于坎卦

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        int shieldAmount = kanData.basicValue + kanData.buffedValue + kanData.tempValue;
        Player player = target as Player;
        if (player == null)
            Debug.Log("Kan Gua can only be applied to player characters.");
        player.UpdateShield(shieldAmount);
    }
}
