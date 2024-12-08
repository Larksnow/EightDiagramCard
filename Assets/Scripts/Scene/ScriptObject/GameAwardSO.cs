using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/GameAward")]
public class GameAwardSO : ScriptableObject
{
    // TODO: May need to change this to a list of awards
    public AwardType awardType = AwardType.Card;
}
