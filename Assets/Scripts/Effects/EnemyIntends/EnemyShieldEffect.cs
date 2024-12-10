using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShieldEffect", menuName = "Enemy Effects/Enemy Shield Effect")]
public class EnemyShieldEffect : EnemyEffect
{
    public override void Execute(EnemyBase sender, CharacterBase player)
    {
       sender.AddShield(Value);
    }
}
