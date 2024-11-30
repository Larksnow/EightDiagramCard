using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MitiEffect", menuName = "Buff/Miti Effect")]
public class MitiEffect : Effect
{
    // 坚硬（受到伤害降低25%，受击后减少一层）
    public DiagramDataSO genData;   // 专属于艮卦

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        int mitiAmount = genData.basicValue + genData.buffedValue + genData.tempValue;
        Player player = target as Player;
        if (player == null)
            Debug.Log("Gen Gua can only be applied to player characters.");
        player.UpdateBuffNumber(BuffType.Miti, mitiAmount);
    }
}
