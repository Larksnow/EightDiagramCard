using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHoldDeck : MonoBehaviour
{
    public CardListPanelController cardListPanelController;
    public float animationDuration = 1f;

    private PauseManager pauseManager;

    private void Awake()
    {
        pauseManager = PauseManager.Instance;
    }

    #region Event Listening
    public void OnClick(object obj)
    {
        PointerEventData pointerEventData = (PointerEventData)obj;
        GameObject selected = pointerEventData.pointerPress;
        if (selected != this) return;

        // 展示玩家牌组
        cardListPanelController.ToggleCardListPanel(CardListType.PlayerHold);
    }

    // 卡牌进入牌组动画
    public void AddCardToDeck(object obj)
    {
        GameObject card = (GameObject)obj;
        Vector3 originalPosition = card.transform.position;
        Vector3 originalScale = card.transform.localScale;

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
