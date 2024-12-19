using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 适合于Canvas渲染的卡牌预览UI
/// </summary>
public class CardPreview : MonoBehaviour
{
    [Header("Card Components")]
    public CardDataSO cardData;
    public TextMeshProUGUI nameText, costText, descriptionTest;
    public Image cardSprite, cardFrame;

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardSprite;
        cardFrame.color = data.color;
        costText.text = data.cost.ToString();
        costText.color = Color.white;
        nameText.text = data.cardName;
        descriptionTest.text = data.cardDescription;
    }
}
