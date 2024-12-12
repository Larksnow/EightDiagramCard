using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IColliderSetUp,IHoverScalable
{
    public GameObject panel;
    public float scaleOnEnter = 1.1f;

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
        panel.SetActive(true);
        if (GetComponent<Button>() == null)
        {
            ((IHoverScalable)this).OnHoverEnter(gameObject, scaleOnEnter * transform.localScale);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
        if (GetComponent<Button>() == null)
        {
            ((IHoverScalable)this).OnHoverExit(gameObject, originalScale);
        }
    }

    #endregion
}