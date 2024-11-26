using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MitiEffect", menuName = "Buff/Miti Effect")]
public class MitiEffect : Effect
{
    // 坚硬（受到伤害降低25%，受击后减少一层）
    int number = 1; //每次该效果施加的层数
    public override void Execute(CharacterBase target, DiagramDataSO diagramData)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                Debug.Log($"Passing {number} to {target}");
                target.UpdateMitiNumber(number);
                break;
            case EffectTargetType.Single:
            case EffectTargetType.All:
                break;
        }
    }

}
