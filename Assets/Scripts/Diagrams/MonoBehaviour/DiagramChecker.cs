using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DiagramChecker : MonoBehaviour
{
    public List<CardDataSO> yaoList;
    private int maxCount = 6;
    public DiagramManager diagramManager;
    public IntEventSO addOneYaoEvent;

    public void updateDiagramChecker(CardDataSO cardData)
    {
        yaoList.Insert(0, cardData);
        // Limit the list to maxCount elements
        if (yaoList.Count > maxCount)
        {
            yaoList.RemoveAt(yaoList.Count - 1); // Remove the first element
        }

        addOneYaoEvent.RaiseEvent((int)cardData.cardType, this);

        checkPattern();
    }

    public void checkPattern()
    {
        if (yaoList.Count < 3) return;
        foreach (var diagram in diagramManager.diagramDataList)
        {
            var upYao = yaoList[0];
            var midYao = yaoList[1];
            var downYao = yaoList[2];
            if (upYao.cardType == diagram.diagramPattern[0] && midYao.cardType == diagram.diagramPattern[1] && downYao.cardType == diagram.diagramPattern[2])
            {
                var yaoArray = new CardDataSO[3] { yaoList[0], yaoList[1], yaoList[2] };
                diagramManager.ApplyCardsEffect(yaoArray, diagram);
                diagramManager.ApplyDiagramEffect(diagram);
            }
        }
    }

    public void ResetChecker()
    {
        yaoList.Clear();
    }
}
