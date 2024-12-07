using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    [Serializable]
    private class ButtonSettings
    {
        public bool interactable = true;
        public bool clickable = true;
        public float scaleOnHover = 1.1f;
        public float scaleOnClick = 1.15f;
    }

    [SerializeField] private ButtonSettings settings;
    [SerializeField] private ObjectEventSO onClickedEvent;
    [SerializeField] private GameObject hoverPanel;

    private Vector3 originalScale;
    private ButtonsManager buttonsManager;

    private void Awake()
    {
        originalScale = transform.localScale;
        buttonsManager = ButtonsManager.Instance;

        if (GetComponent<BoxCollider2D>() == null)
            SetupCollider();
    }

    private void Update()
    {
        UpdateInteractableState();
    }

    private void OnEnable()
    {
        buttonsManager.AddButton(this); 
    }

    private void OnDisable()
    {
        buttonsManager.RemoveButton(this); 
    }

    private void UpdateInteractableState()
    {
        settings.interactable = buttonsManager.IsButtonInteractable(this);
    }

    #region Pointer Event Handlers
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!settings.interactable) return;

        if (settings.clickable)
            transform.localScale *= settings.scaleOnHover;

        if (hoverPanel != null)
            hoverPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!settings.interactable) return;

        if (settings.clickable)
            transform.localScale = originalScale;

        if (hoverPanel != null)
            hoverPanel.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!settings.interactable || !settings.clickable) return;
        transform.localScale = originalScale * settings.scaleOnClick;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!settings.interactable || !settings.clickable) return;

        transform.localScale = originalScale;
        onClickedEvent?.RaiseEvent(eventData, this);
    }
    #endregion

    private void SetupCollider()
    {
        var collider = gameObject.AddComponent<BoxCollider2D>();

        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            SetupSpriteCollider(collider, spriteRenderer);
        }
        else if (TryGetComponent<TextMeshPro>(out var textMeshPro))
        {
            collider.size = textMeshPro.bounds.size;
        }
        else
        {
            collider.size = transform.lossyScale;
        }
    }

    private void SetupSpriteCollider(BoxCollider2D collider, SpriteRenderer spriteRenderer)
    {
        Rect spriteRect = spriteRenderer.sprite.rect;
        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        Vector2 actualSize = new(
            spriteRect.width / pixelsPerUnit,
            spriteRect.height / pixelsPerUnit
        );
        collider.size = actualSize;
    }
}