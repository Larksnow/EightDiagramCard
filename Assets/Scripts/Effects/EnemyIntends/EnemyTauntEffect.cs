using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTauntEffect", menuName = "Enemy Effects/Enemy Taunt Effect")]
public class EnemyTauntEffect : EnemyEffect
{
    public override void Execute(EnemyBase sender, CharacterBase player)
    {
       sender.isTaunting = true;
    }
}
