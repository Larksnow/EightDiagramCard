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

    private void Awake()
    {
        currentCard = GetComponent<Card>();
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
            //TODO:Excute Card effect
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isAnimating = false;
        }
    }
}
