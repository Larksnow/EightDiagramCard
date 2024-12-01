using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDamageEffect", menuName = "Enemy Effects/Enemy Damage Effect")]
public class EnemyDamageEffect : EnemyEffect
{
    public EnemyDataSO enemyData;
    public int damageTimes;
    public int damageRatio;
    public override void Execute(EnemyBase sender, CharacterBase player) // first parameter is the caller of the effect
    {
       int damageAmount = Mathf.CeilToInt(damageRatio * enemyData.currentAttack);
       for(int i = 0; i < damageTimes; i++)
       {
            player.TakeDamage(damageAmount, sender, false);
       }
    }
}
