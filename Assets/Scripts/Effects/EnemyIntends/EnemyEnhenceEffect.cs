using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyEnhenceEffect", menuName = "Enemy Effects/Enemy Enhence Effect")]
public class EnemyEnhenceEffect : EnemyEffect
{
    public EnemyDataSO enemyData;
    public override void Execute(EnemyBase sender, CharacterBase player)
    {
       enemyData.currentAttack += Value;
    }
}
