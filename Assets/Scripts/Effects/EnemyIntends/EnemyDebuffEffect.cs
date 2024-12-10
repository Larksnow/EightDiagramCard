using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDebuffEffect", menuName = "Enemy Effects/Enemy Debuff Effect")]
public class EnemyDebuffEffect : EnemyEffect
{
    public List<BuffType> buffs;
    public override void Execute(EnemyBase sender, CharacterBase player)
    {
       for (int i = 0; i < buffs.Count; i++)
       {
           player.AddBuffNumber(buffs[i], Value);
       }
    }
}
