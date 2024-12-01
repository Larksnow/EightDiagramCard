using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTurnAction", menuName = "EnemyTurnAction", order = 1)]
public class EnemyTurnAction : ScriptableObject
{
    public List<EnemyEffect> actionList;
}
