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
    public DiagramChecker diagramChecker;

    private PauseManager pauseManager;

    public ObjectEventSO lackOfManaEvent;

    private void Awake()
    {
        diagramChecker = GameObject.Find("DiagramChecker").GetComponent<DiagramChecker>();
        pauseManager = PauseManager.Instance;
        currentCard = GetComponent<Card>();
    }

    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (pauseManager.IsPaused()) return;
        if (!currentCard.isMouseOver) return;
        if (currentCard.isAnimating) return;
        if (!currentCard.isAvailable)
        {
            // 法力不足，无法打出
            lackOfManaEvent.RaiseEvent(null, this);
        };

        canMove = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentCard.isAnimating) return;
        if (!currentCard.isAvailable) return;
        if (canMove)
        {
            currentCard.isDraging = true;
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currentCard.transform.position = worldPos;
            canExecute = worldPos.y > 0f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentCard.isAnimating) return;
        if (!currentCard.isAvailable) return;
        if (canExecute && currentCard.isDraging && canMove)
        {
            currentCard.isDraging = false;
            CardDataSO yao = currentCard.cardData;
            // Cards only affect player himself
            currentCard.ExecuteCardEffect(currentCard.player);
            diagramChecker.updateDiagramChecker(yao);
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isDraging = false;
        }
    }
}
