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
            // if diagramData.effects don't contains this effect, inset it
            if (!diagramData.effects.Contains(effectToAdd))
            {
                // Insert the effect at the start of the list if it's not already there
                diagramData.effects.Insert(0, effectToAdd);
            }
            else // If it already exists, increase its number by 1
            {
                effectToAdd.tempValue += 1;
            }
        }
        else
        {
            Debug.LogError("NO Diagram Data or Diagram Effect assigned to effect: " + this.name);
        }
    }
}
