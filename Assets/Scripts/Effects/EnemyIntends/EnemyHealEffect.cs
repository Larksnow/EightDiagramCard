using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyHealEffect", menuName = "Enemy Effects/Enemy Heal Effect")]
public class EnemyHealEffect : EnemyEffect
{
    public override void Execute(EnemyBase sender, CharacterBase player)
    {
       sender.AddHP(value);
    }
}
