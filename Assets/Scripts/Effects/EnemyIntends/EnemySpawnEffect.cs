using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnEffect", menuName = "Enemy Effects/Enemy Spawn Effect")]
public class EnemySpawnEffect : EnemyEffect
{
    public ObjectEventSO enemySpawnEvent;
    public override void Execute(EnemyBase sender, CharacterBase player)
    {
       enemySpawnEvent.RaiseEvent(null, this); //TODO: spawn enemy
    }
}
