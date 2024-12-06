using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class Button : MonoBehaviour, IPointerInteractionHandler
{
    [System.Serializable]
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
    private PauseManager pauseManager;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        originalScale = transform.localScale;
        pauseManager = PauseManager.Instance;

        if (GetComponent<BoxCollider2D>() == null)
            SetupCollider();
    }

    private void Update()
    {
        UpdateInteractableState();
    }

    private void UpdateInteractableState()
    {
        settings.interactable = !pauseManager.IsPaused() ||
                               pauseManager.IsInExcludeList(gameObject);
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

    public void FadeOutAfterClick(FadeInOutHander fadeInOutHander, Action onComplete = null)
    {
        StartCoroutine(FadeOutRoutine(fadeInOutHander, onComplete));
    }

    private IEnumerator FadeOutRoutine(FadeInOutHander fadeInOutHander, Action onComplete)
    {
        settings.interactable = false;
        fadeInOutHander.FadeOut();
        yield return new WaitForSeconds(fadeInOutHander.fadeDuration);
        settings.interactable = true;
        onComplete?.Invoke();
    }

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

    public void SetInteractable(bool interactable)
    {
        settings.interactable = interactable;
    }
}

public interface IPointerInteractionHandler :
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerClickHandler
{
}