using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class EndTurnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GamePlayPanel gamePlayPannel;
    private Vector3 originalScale;
    public float hoverScaleMultiplier = 1.1f; // Scale multiplier for hover
    public float clickScaleMultiplier = 1.2f; // Scale multiplier for click
    public float animationDuration = 0.1f;

    private bool isRotating;
    public bool pressEnabled;
    private PauseManager pauseManager;

    private void Awake()
    {
        pauseManager = PauseManager.Instance;
        originalScale = transform.localScale;
        gamePlayPannel = FindObjectOfType<GamePlayPanel>();
    }
    public void OnMouseDown()
    {
        if (isRotating) return;
        if (pauseManager.IsPaused()) return;
        if (!pressEnabled) return;
        Sequence sequence = DOTween.Sequence();
        // Scale up
        sequence.Append(transform.DOScale(originalScale * clickScaleMultiplier, animationDuration).SetEase(Ease.OutExpo));
        // Scale back to original size
        sequence.Append(transform.DOScale(originalScale, 1).SetEase(Ease.OutExpo));
        RotateEndTurnButton();
        gamePlayPannel.OnEndTurnButtonClicked();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isRotating) return;
        if (pauseManager.IsPaused()) return;
        transform.DOScale(originalScale * hoverScaleMultiplier, animationDuration).SetEase(Ease.OutExpo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isRotating) return;
        if (pauseManager.IsPaused()) return;
        transform.DOScale(originalScale, animationDuration).SetEase(Ease.OutExpo);
    }

    public void RotateEndTurnButton()
    {
        if (isRotating) return;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, 180);
        transform.DORotateQuaternion(targetRotation, 1).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            isRotating = false;
        });
    }
}
