using System;
using UnityEngine;
//This is the base class for all effects

public abstract class Effect : ScriptableObject
{
    public int value;

    public EffectTargetType targetType;

    public abstract void Execute(CharacterBase target, DiagramDataSO diagramData);
}