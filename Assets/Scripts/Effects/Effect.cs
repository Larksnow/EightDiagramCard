using System;
using UnityEngine;
//This is the base class for all effects

public abstract class Effect : ScriptableObject
{
    public int value;

    public EffectTargetType targetType;

    // CardEffect执行，'triggered'是该Card组成的卦象
    public abstract void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0);
}