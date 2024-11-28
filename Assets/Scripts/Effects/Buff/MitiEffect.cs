using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MitiEffect", menuName = "Buff/Miti Effect")]
public class MitiEffect : Effect
{
    // 坚硬（受到伤害降低25%，受击后减少一层）
    public int buffNumber = 1; // 有buff时额外层数
    public DiagramDataSO genData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        int mitiAmount = value;
        if (genData.yangBuff || genData.yinBuff) 
        {
            mitiAmount += buffNumber;
        }
        target.UpdateMitiNumber(mitiAmount);
    }
}
