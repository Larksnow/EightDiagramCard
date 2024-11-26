using System;
using UnityEngine;
//This is the base class for all effects

public abstract class Effect : ScriptableObject
{
    public int value;

    public EffectTargetType targetType;

    public abstract void Execute(CharacterBase target);
    public void Execute(DiagramDataSO diagramData) {
        // 影响卦象的效果
    }
}