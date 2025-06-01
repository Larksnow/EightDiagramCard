using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IColliderSetUp, IScaleOnMouseOver
{
    public GameObject panel;
    public float scaleOnEnter = 1.1f;
    public MouseOverAnimationSO mouseOverAnimationSO; // 鼠标进入/退出该物体时的缓动效果

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;

        if (GetComponent<BoxCollider2D>() == null)
            ((IColliderSetUp)this).SetupCollider(gameObject);
    }

    #region Pointer Event Handlers

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panel != null) panel.SetActive(true);
        if (GetComponent<Button>() == null)
        {
            ((IScaleOnMouseOver)this).OnMouseEnter(gameObject, scaleOnEnter * transform.localScale,
                mouseOverAnimationSO);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (panel != null) panel.SetActive(false);
        if (GetComponent<Button>() == null)
        {
            ((IScaleOnMouseOver)this).OnMouseExit(gameObject, originalScale, mouseOverAnimationSO);
        }
    }

    #endregion
}