using System;
using UnityEngine;
using UnityEngine.Serialization;

//This is the base class for all effects

public abstract class EnemyEffect : ScriptableObject
{
    [SerializeField] protected int value;
    public virtual int Value => value;

    public Sprite icon;
    [TextArea] public string description;
    public EnemyEffectType effectType;

    public abstract void Execute(EnemyBase sender, CharacterBase player);
    // TODO: reset effect
}