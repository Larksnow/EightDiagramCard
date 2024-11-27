using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiagramData", menuName = "Diagram/DiagramData")]
public class DiagramDataSO : ScriptableObject
{
    public string diagramName;
    public Color diagramColor;
    public Sprite patternSprite;
    public Sprite nameSprite;
    [TextArea]
    public string description;
    public DiagramType diagramType;
    public List<CardType> diagramPattern; //TODO: 卦的图案
    public bool yinBuff; // 卦可以被阴阳爻增益
    public bool yangBuff;

    public List<Effect> effects;
}
