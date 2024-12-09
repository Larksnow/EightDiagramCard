using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiagramData", menuName = "Diagram/DiagramData")]
public class DiagramDataSO : ScriptableObject
{
    public DiagramEffect basicEffect;
    public string diagramName;
    public Color diagramColor;
    public Sprite patternSprite;
    public int triggerTime; // 触发次数
    public int defaultValue; // 卦的初始数值
    public int basicValue; // 卦的基础数值
    public int buffedValue; // 卦的增益数值，每场战斗后重置
    public int tempValue; // 卦的临时增益，触发一次卦象后清零

    [TextArea]
    public string description;
    public DiagramSO diagramType;
    public List<CardType> diagramPattern; // 卦的图案

    public List<DiagramEffect> effects;// 存储卦的所有效果，基础，祝福，卡牌增益等

    public virtual void ResetToDefault()// Reset when New Game Start
    {
        buffedValue = 0;
        tempValue = 0;
        basicValue = defaultValue;
        triggerTime = 1;
        effects.Clear();
        effects.Add(basicEffect);
    }

    public virtual void ResetAfterBattle()
    {
        buffedValue = 0;
        ResetAfterTrigger();
    }

    public virtual void ResetAfterTrigger()
    {
        tempValue = 0;
        triggerTime = 1;
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i].isTemp)
                effects.Remove(effects[i]);
        }
    }
}
