using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CopyDiagramEffect", menuName = "Diagram Effects/Copy Diagram Effect")]
public class CopyDiagramEffect : Effect
{
    public DiagramManager diagramManager;
    public ObjectEventSO selectDiagramEvent;
    public DiagramDataSO diagramDataToCopy = null;

    private DiagramDataSO qianData;

    /// 复制除乾卦外的其他任何一种卦象打出
    public override void Execute(CharacterBase target, DiagramDataSO diagramData)
    {
        if (diagramData.diagramType != DiagramType.Qian)
            Debug.LogWarning("CopyDiagramEffect can only be used with Qian diagram");

        qianData = diagramData;
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
        diagramManager.StartCoroutine(WaitForCopyDiagram());
    }

    private IEnumerator WaitForCopyDiagram()
    {
        diagramDataToCopy = null;
        selectDiagramEvent.RaiseEvent(null, this);
        yield return new WaitUntil(() => diagramDataToCopy != null);
        // 如果有buff加成，触发两次
        if (qianData.yangBuff || qianData.yinBuff)
        {

            diagramManager.ApplyDiagramEffect(diagramDataToCopy);
            // 触发后清除卦上的buff
            qianData.yinBuff = false;
            qianData.yangBuff = false;
        }
        diagramManager.ApplyDiagramEffect(diagramDataToCopy);
    }

}