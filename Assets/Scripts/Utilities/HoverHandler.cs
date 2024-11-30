using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 处理鼠标悬停事件
/// </summary>
public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    [Header("Broadcast Events")]
    public ObjectEventSO onClickedEvent;

    public bool interactable;
    public float scaleOnHover;
    public float scaleOnClick;

    [Header("Hover Panel")]
    public bool hasHoverPanel;
    public GameObject hoverPanel;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable)
        {
            transform.localScale *= scaleOnHover;
        }
        if (hasHoverPanel)
        {
            hoverPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (interactable)
        {
            transform.localScale = originalScale;
        }
        if (hasHoverPanel)
        {
            hoverPanel.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (interactable)
        {
            transform.localScale = originalScale * scaleOnClick;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            transform.localScale = originalScale;
            onClickedEvent.RaiseEvent(eventData, this);
        }
    }
}