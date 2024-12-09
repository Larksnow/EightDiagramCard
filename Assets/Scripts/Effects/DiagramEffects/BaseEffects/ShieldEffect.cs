using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldEffect", menuName = "Diagram Effects/Shield Effect")]
public class ShieldEffect : DiagramEffect
{
    // 化劲(护甲，可以直接抵消敌人的伤害, 下回合开始多余的化劲会清空)

    public override void Execute(CharacterBase target)
    {
        Player player = target as Player;
        if (player == null)
            Debug.Log("Kan Gua can only be applied to player characters.");
        player.AddShield(value);
    }
}
