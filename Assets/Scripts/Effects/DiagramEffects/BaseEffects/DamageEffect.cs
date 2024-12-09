using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Diagram Effects/Damage Effect")]

public class DamageEffect : DiagramEffect
{
    public override void Execute(CharacterBase target)
    {
        CharacterBase attacker = GameObject.FindGameObjectWithTag("player").GetComponent<CharacterBase>();
        // 伤害向上取整
        for (int i = 0; i < diagramData.triggerTime; i++)
        {
            target.TakeDamage(value, attacker, false);
        }
        diagramData.tempValue = 0; //重置临时值
    }
}