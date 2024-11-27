using System;
using UnityEngine;
//This is the base class for all effects

public abstract class Effect : ScriptableObject
{
    public int value;

    public EffectTargetType targetType;

    /// <summary>
    /// This method is called when the effect is executed on a target.
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="diagramData">作为CardEffect执行时为被加成的卦象，作为DiagramEffect执行时为当前卦象</param>
    public abstract void Execute(CharacterBase target, DiagramDataSO diagramData);
}