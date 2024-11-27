using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CopyDiagramEffect", menuName = "Diagram Effects/Copy Diagram Effect")]
public class CopyDiagramEffect : Effect
{
    public DiagramManager diagramManager;
    public ObjectEventSO selectDiagramEvent;
    public DiagramDataSO diagramDataToCopy = null;

    /// 复制除乾卦外的其他任何一种卦象打出
    public override void Execute(CharacterBase target)
    {
        if (diagramManager == null)
        {
            GameObject diagramManagerObject = GameObject.Find("DiagramManager");
            if (diagramManagerObject!= null)
            {
                diagramManager = diagramManagerObject.GetComponent<DiagramManager>();
            }
        }
        if (diagramManager == null)
        {
            Debug.Log("DiagramManager not found");
        }
        diagramManager.StartCoroutine(WaitForCopyDiagram());
    }

    private IEnumerator WaitForCopyDiagram()
    {
        diagramDataToCopy = null;
        selectDiagramEvent.RaiseEvent(null, this);
        yield return new WaitUntil(() => diagramDataToCopy != null);
        diagramManager.ApplyDiagramEffect(diagramDataToCopy);
    }

    public override void Execute(DiagramDataSO diagramData)
    {
        throw new System.NotImplementedException();
    }
}