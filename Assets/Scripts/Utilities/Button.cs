using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
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

        if (GetComponent<BoxCollider2D>() == null) SetupCollider();
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

    // 在监听处调用(点击后自动淡出效果)
    public void FadeOutAfterClick(FadeInOutHander fadeInOutHander, Action onComplete = null)
    {
        StartCoroutine(FadeOutAfterClickCoroutine(fadeInOutHander, onComplete));
    }
    private IEnumerator FadeOutAfterClickCoroutine(FadeInOutHander fadeInOutHander, Action onComplete = null)
    {
        interactable = false;
        fadeInOutHander.FadeOut();
        yield return new WaitForSeconds(fadeInOutHander.fadeDuration);
        interactable = true;
        onComplete?.Invoke();
    }

    // 自动添加Button所需的BoxCollider2D组件
    private void SetupCollider()
    {
        var collider = gameObject.AddComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        if (spriteRenderer != null)
        {
            Rect spriteRect = spriteRenderer.sprite.rect;
            Vector2 actualSize = new(
                spriteRect.width / spriteRenderer.sprite.pixelsPerUnit,
                spriteRect.height / spriteRenderer.sprite.pixelsPerUnit
            );
            collider.size = actualSize;
        }
        else if (textMeshPro != null)
        {
            collider.size = textMeshPro.bounds.size;
        }
        else
        {
            collider.size = transform.lossyScale;
        }
    }
}