using System;
using UnityEngine;
//This is the base class for all effects

public abstract class EnemyEffect : ScriptableObject
{
    public int value;
    public Sprite icon;
    [TextArea]
    public string description;

    public abstract void Execute(EnemyBase sender, CharacterBase player);
    // TODO: reset effect
    
    public virtual int GetRuntimeValue()
    {
        return value;
    }
}