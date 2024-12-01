using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBuffEffect", menuName = "Enemy Effects/Enemy Buff Effect")]
public class EnemyBuffEffect : EnemyEffect
{
    public List<BuffType> buffs;
    public override void Execute(EnemyBase sender, CharacterBase player)
    {
       for (int i = 0; i < buffs.Count; i++)
        {
            sender.AddBuffNumber(buffs[i], value);
            sender.AddBuffNumber(buffs[i], value);
        }
    }
}
