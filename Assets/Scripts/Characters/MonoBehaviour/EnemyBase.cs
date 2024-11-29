using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    [Header("Buffs")]
    public bool isTaunting;    // 嘲讽(使敌方只能攻击此单位)

    public override void OnTurnBegin()
    {
        base.OnTurnBegin();
        // TODO: 嘲讽只生效一回合
        if (isTaunting) isTaunting = false;
    }
}
