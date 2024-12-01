using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHoldDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardListPanelController cardListPanelController;
    public float animationDuration = 1.5f;

    private void Awake()
    {
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
    }

    #region Event Listening
    public void OnClick(object obj)
    {
        PointerEventData pointerEventData = (PointerEventData)obj;
        GameObject selected = pointerEventData.pointerPress;
        if (selected != this) return;

        // TODO:展示玩家牌组
        cardListPanelController.ToggleCardListPanel(CardListType.PlayerHold);
    }

    public void AddCardToDeck(object obj)
    {
        GameObject card = (GameObject)obj;
        Vector3 originalPosition = card.transform.position;
        Vector3 originalScale = card.transform.localScale;

        // 卡牌进入牌组动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(card.transform.DOMove(transform.position, animationDuration))
        .Join(card.transform.DOScale(0, animationDuration))
        .OnComplete(() =>
        {
            card.transform.position = originalPosition;
            card.transform.localScale = originalScale;
        });
    }
    #endregion
}
