using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// 适合于Canvas渲染的卡牌预览UI
/// </summary>
public class CardPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScaleOnMouseOver
{
    [Header("Card Components")] public CardDataSO cardData;
    public TextMeshProUGUI nameText, costText, descriptionTest;
    public Image cardSprite, cardFrame;

    [Header("Mouse Over Animation")] public MouseOverAnimationSO mouseOverAnimationSO;
    public float scaleOnEnter = 1.3f;

    private Vector3 originalScale;

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardSprite;
        cardFrame.color = data.color;
        costText.text = data.cost.ToString();
        costText.color = Color.white;
        nameText.text = data.cardName;
        descriptionTest.text = data.cardDescription;
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IScaleOnMouseOver)this).OnMouseEnter(gameObject, scaleOnEnter * transform.localScale, mouseOverAnimationSO);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IScaleOnMouseOver)this).OnMouseExit(gameObject, originalScale, mouseOverAnimationSO);
    }
}