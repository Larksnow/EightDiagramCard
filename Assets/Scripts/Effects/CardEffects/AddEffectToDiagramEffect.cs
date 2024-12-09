using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddEffectToDiagramEffect", menuName = "Card Effects/Add Effect To Diagram Effect")]
public class AddEffectToDiagramEffect : CardEffect
{
    public DiagramDataSO diagramData;
    public DiagramEffect effectToAdd;
    public override void Execute(DiagramDataSO triggeredDiagram = null)
    {
        if (diagramData != null && effectToAdd != null)
        {
            diagramData.effects.Insert(0, effectToAdd);
        }
        else
        {
            Debug.LogError("NO Diagram Data or Diagram Effect assigned to effect: " + this.name);
        }
    }
}
