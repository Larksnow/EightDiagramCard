using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiagramEffect : ScriptableObject
{
    public DiagramDataSO diagramData;
    public int value
    {
        get => Mathf.CeilToInt(diagramData.basicValue + diagramData.buffedValue + diagramData.tempValue);
    }

    public int tempValue;
    public bool isTemp; // true if the effect is temporary and should be removed after trigger
    public EffectTargetType currentTargetType;
    public EffectTargetType defaultTargetType;
    [TextArea]
    public string description;
    public abstract void Execute(CharacterBase target);
    public virtual string GetFormattedDescription()
    {
        return description.Replace("{value}", value.ToString());
    }
    public virtual void ResetEffect()
    {
        currentTargetType = defaultTargetType;
    }

    public virtual void RestEffectAfterTrigger()
    {
        tempValue = 1;
    }
}
