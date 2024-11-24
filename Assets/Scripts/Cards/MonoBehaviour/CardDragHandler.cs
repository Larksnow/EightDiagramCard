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

    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable) return;
        canMove = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable) return;
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
        if (!currentCard.isAvailable) return;
        if(canExecute)
        {
            CardType yao = currentCard.cardData.cardType;
            // Cards only affect player himself
            currentCard.ExecuteCardEffect(currentCard.player); 
            diagramChecker.updateDiagramChecker(yao);
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isAnimating = false;
        }
    }
}
