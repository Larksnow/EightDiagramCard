using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XunData", menuName = "Diagram/XunData")]
public class XunDataSO : DiagramDataSO
{
    public Dictionary<CardType, int> specialCardsToDraw;
    public override void RestToDefault()
    {
        buffedValue = 0;
        tempValue = 0;
        effects.Clear();
        effects.Add(basicEffect);
        specialCardsToDraw.Clear();
    }
}
