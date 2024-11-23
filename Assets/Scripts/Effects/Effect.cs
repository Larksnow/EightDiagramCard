using System;
using UnityEngine;
//This is the base class for all effects

public abstract class Effect : ScriptableObject
{
    public int value;

    public EffectTargetType targetType;

    //TODO: 看看 from有没有必要，不然就删除
    public abstract void Execute(CharacterBase from, CharacterBase target);
}