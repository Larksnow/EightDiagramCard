using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CopyDiagramEffect", menuName = "Diagram Effects/Copy Diagram Effect")]
public class CopyDiagramEffect : Effect
{
    public DiagramManager diagramManager;
    public ObjectEventSO selectDiagramEvent;
    public DiagramDataSO diagramDataToCopy = null;
    public DiagramDataSO qianData;  // 专属于乾卦
    public HashSet<DiagramDataSO> enhancedCopyDiagram = new();


    // 复制除乾卦外的其他任何一种卦象打出
    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
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
            diagramManager.ApplyDiagramEffect(diagramDataToCopy);
            // TODO: buff重置
        }
        diagramManager.ApplyDiagramEffect(diagramDataToCopy);
        enhancedCopyDiagram.Clear();
    }
}