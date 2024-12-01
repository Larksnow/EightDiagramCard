using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    public int defaultAttack; //默认攻击力
    public int currentAttack; //当前功击
    public List<EnemyTurnAction> dayIntends;
    public List<EnemyTurnAction> nightIntends;

    public Sprite daySprite;
    public Sprite nightSprite;

    public virtual void Reset()
    {
        currentAttack = defaultAttack; //重置攻击力
    }
}
