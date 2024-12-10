using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "EnemyDamageEffect", menuName = "Enemy Effects/Enemy Damage Effect")]
public class EnemyDamageEffect : EnemyEffect
{
    public EnemyDataSO enemyData;
    public int damageTimes;
    public int damageRatio;

    public override int Value => Mathf.CeilToInt(damageRatio * enemyData.currentAttack);

    public override void Execute(EnemyBase sender, CharacterBase player) // first parameter is the caller of the effect
    {
        for (int i = 0; i < damageTimes; i++)
        {
            player.TakeDamage(Value, sender, false);
        }
    }
}