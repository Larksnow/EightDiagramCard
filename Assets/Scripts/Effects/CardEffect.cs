using System;
using UnityEngine;
//This is the base class for all effects

public abstract class CardEffect : ScriptableObject
{
    public int value;
    [TextArea]
    public string description;
    // CardEffect执行，'triggered'是该Card组成的卦象
    public abstract void Execute(DiagramDataSO triggeredDiagram = null);
}