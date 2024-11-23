using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiagramData", menuName = "Diagram/DiagramData")]
public class DiagramDataSO : ScriptableObject
{
    public string diagramName;
    public Sprite patternSprite;
    public Sprite nameSprite;
    [TextArea]
    public string description;
    public DiagramType diagramType; 
    public List<CardType> diagramPattern; //TODO: 卦的图案
    
    public List<Effect> effects;
}
