using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 处理鼠标悬停事件
/// </summary>
public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    public bool interactable;   // 是否响应鼠标事件

    [Header("Clickable")]
    public bool clickable;      // 是否可点击，鼠标悬停是否变大
    public float scaleOnHover;
    public float scaleOnClick;
    public ObjectEventSO onClickedEvent;

    [Header("Hover Panel")]
    public bool hasHoverPanel;
    public GameObject hoverPanel;

    private PauseManager pauseManager;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        pauseManager = PauseManager.Instance;
    }

    private void Update()
    {
        if (pauseManager.IsPaused())
        {
            if (!pauseManager.IsInExcludeList(gameObject))
            {
                interactable = false;
            }
        }
        else if (!interactable)
        {
            interactable = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable)
        {
            if (clickable)
            {
                transform.localScale *= scaleOnHover;
            }
            if (hasHoverPanel)
            {
                hoverPanel.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (interactable)
        {
            if (clickable)
            {
                transform.localScale = originalScale;
            }
            if (hasHoverPanel)
            {
                hoverPanel.SetActive(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (interactable)
        {
            if (clickable)
            {
                transform.localScale = originalScale * scaleOnClick;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            if (clickable)
            {
                transform.localScale = originalScale;
                onClickedEvent.RaiseEvent(eventData, this);
            }
        }
    }

}