using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Card currentCard;
    private bool canMove;
    private bool canExecute;

    private DiagramChecker diagramChecker;

    private void Awake()
    {
        currentCard = GetComponent<Card>();
        diagramChecker = GameObject.Find("DiagramChecker").GetComponent<DiagramChecker>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canMove = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canMove)
        {
           currentCard.isAnimating = true;
           Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);
           Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
           currentCard.transform.position = worldPos;
           canExecute = worldPos.y > 0f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(canExecute)
        {
            //TODO: 1.Excute Card effect
            //TODO: 2.更新计数器，计数器里再触发卦的效果 
            diagramChecker.updateDiagramChecker(currentCard);
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isAnimating = false;
        }
    }
}
