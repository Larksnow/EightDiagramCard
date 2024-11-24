using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DiagramChecker : MonoBehaviour
{
    public List<CardType> yaoList;
    private int maxCount = 6;

    public Card cardPlayed;
    public DiagramManager diagramManager;

    public void updateDiagramChecker(CardType cardType)
    {
        yaoList.Insert(0, cardType);
        // Limit the list to maxCount elements
        if (yaoList.Count > maxCount)
        {
            yaoList.RemoveAt(yaoList.Count - 1); // Remove the first element
        }
        checkPattern();
    }

    public void checkPattern()
    {
        if (yaoList.Count < 3) return;
        foreach (var item in diagramManager.diagramDataList)
        {
            var upYao = yaoList[0];
            var midYao = yaoList[1];
            var downYao = yaoList[2];
            if (upYao == item.diagramPattern[0] && midYao == item.diagramPattern[1] && downYao == item.diagramPattern[2])
            {
                Debug.Log("Pattern Matched: " + item.diagramName);
                diagramManager.ApplyDiagramEffect(item);
            }
        } 
    }
}