using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Diagram Effects/Damage Effect")]

public class DamageEffect : Effect
{
    
    public DiagramDataSO diagramData;
    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        CharacterBase attacker = GameObject.FindGameObjectWithTag("player").GetComponent<CharacterBase>();
        // 伤害向上取整
        var damageValue = Mathf.CeilToInt(diagramData.basicValue + diagramData.buffedValue + diagramData.tempValue);
        for (int i = 0; i < diagramData.triggerTime; i++)
        {
            target.TakeDamage(damageValue, attacker, false);
        }
        diagramData.triggerTime = 1; //重置触发次数
    }
}