using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "CopyDiagramEffect", menuName = "Diagram Effects/Copy Diagram Effect")]
public class CopyDiagramEffect : DiagramEffect
{
    public DiagramManager diagramManager;
    public ObjectEventSO selectDiagramEvent;
    public DiagramDataSO diagramDataToCopy = null;
    public HashSet<DiagramDataSO> enhancedCopyDiagram = new();


    // 复制除乾卦外的其他任何一种卦象打出
    public override void Execute(CharacterBase target)
    {
        if (diagramManager == null)
        {
            GameObject diagramManagerObject = GameObject.Find("DiagramManager");
            if (diagramManagerObject != null)
            {
                diagramManager = diagramManagerObject.GetComponent<DiagramManager>();
            }
        }
        if (diagramManager == null)
        {
            Debug.Log("DiagramManager not found");
        }
        diagramManager.StartCoroutine(WaitForCopyDiagramAndExecute());
    }

    private IEnumerator WaitForCopyDiagramAndExecute()
    {
        diagramDataToCopy = null;
        selectDiagramEvent.RaiseEvent(null, this);
        yield return new WaitUntil(() => diagramDataToCopy != null);
        // 如果此时触发的是被复制加成过的卦象，则触发两次
        if (enhancedCopyDiagram.Contains(diagramDataToCopy))
        {
            diagramDataToCopy.triggerTime += 1;
        }
        diagramManager.ApplyDiagramEffect(diagramDataToCopy);
        enhancedCopyDiagram.Clear();
    }

    public override string GetFormattedDescription()
    {
        string enhancedPart = GetEnhancedDiagramNames();
        return string.IsNullOrEmpty(enhancedPart) ? description : $"{description} {enhancedPart}";
    }

    private string GetEnhancedDiagramNames()
    {
        if (enhancedCopyDiagram == null || enhancedCopyDiagram.Count == 0)
            return string.Empty;
        // Concatenate names of diagrams
        return $"({string.Join("、", enhancedCopyDiagram.Select(d => d.diagramName))} 已增强!)";
    }
}