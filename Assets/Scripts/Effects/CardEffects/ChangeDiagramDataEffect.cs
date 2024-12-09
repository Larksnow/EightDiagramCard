using UnityEngine;

[CreateAssetMenu(fileName = "ChangeDiagramDataEffect", menuName = "Card Effects/Change Diagram Data Effect")]
public class ChangeDiagramDataEffect : CardEffect
{
    public EnhenceDataType enhenceDataType;
    public DiagramDataSO diagramData;

    public override void Execute(DiagramDataSO triggeredDiagram = null)
    {
        if (diagramData != null)
        {
            switch (enhenceDataType)
            {
                case EnhenceDataType.Basic:
                    triggeredDiagram.basicValue += value;
                    break;
                case EnhenceDataType.Buffed:
                    triggeredDiagram.buffedValue += value;
                    break;
                case EnhenceDataType.Temp:
                    triggeredDiagram.tempValue += value;
                    break;
                case EnhenceDataType.TriggerTime:
                    triggeredDiagram.triggerTime += value;
                    break;
            }
        }
        else
        {
            Debug.LogError("No diagram data assigned to effect: " + this.name);
        }
    }
}