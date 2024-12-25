using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

//This script controls all the UI elements in the battle scene
public class GamePlayPanel : MonoBehaviour
{
    // 在这里引用各种UI 
    public GameAwardSO gameAwardData;
    [Header("UI Objects")] public GameObject drawDeckUI;
    public GameObject discardDeckUI;
    public GameObject endTurnButton;
    public GameObject manaUI;
    public TextMeshPro moneyUI;
    public GameObject diagramPannel;
    public GameObject dialogBox;
    public GameObject selectDiagramPannel;
    public GameObject selectCardPannel;
    public GameObject selectLevelPannel;

    [FormerlySerializedAs("cardListDisplayController")]
    public CardDeckPreviewController cardDeckPreviewController;

    public float uiFadeDuration;
    public float dialogBoxDuration;

    private SpriteRenderer manaImage;
    private TextMeshPro manaAmountText;
    private bool hasAvailableCard;


    [Header("Broadcast Events")] public ObjectEventSO playerTurnEndEvent;

    private void OnEnable()
    {
        manaImage = manaUI.GetComponentInChildren<SpriteRenderer>();
        manaAmountText = manaUI.GetComponentInChildren<TextMeshPro>();
    }

    #region End Turn Button

    public void OnEndTurnButtonClicked()
    {
        // Rest DiagramePannel When player turn ended
        diagramPannel.GetComponent<DiagramPanel>().ResetDiagramPannel();
        endTurnButton.GetComponent<EndTurnButton>().pressEnabled = false;
        playerTurnEndEvent.RaiseEvent(null, this);
    }

    public void OnEnemyTurnBegin()
    {
        // endTurnButton.GetComponent<EndTurnButton>().pressEnabled = false;
    }

    public void OnPlayerTurnBegin()
    {
        // endTurnButton.GetComponent<EndTurnButton>().RotateEndTurnButton();
        endTurnButton.GetComponent<EndTurnButton>().pressEnabled = true;
    }

    #endregion

    #region Card Deck UI

    public void UpdateDrawDeckAmount(int amount)
    {
        UpdateDeckAmount(drawDeckUI, amount);
    }

    public void UpdateDiscardDeckAmount(int amount)
    {
        UpdateDeckAmount(discardDeckUI, amount);
    }

    private void UpdateDeckAmount(GameObject cardDeck, int amount)
    {
        TextMeshPro number = cardDeck.GetComponentInChildren<TextMeshPro>();
        int currentAmount = int.Parse(number.text);
        if (currentAmount == amount) return;
        number.text = amount.ToString();

        // 先放大，然后恢复到原大小
        Transform uiTransform = cardDeck.transform;
        uiTransform.DOScale(1.1f, uiFadeDuration / 6).SetEase(Ease.OutCubic)
            .OnComplete(() => { uiTransform.DOScale(1f, uiFadeDuration / 6).SetEase(Ease.OutCubic); });
        Debug.Log("UpdateDeckAmount " + cardDeck.name);
    }

    #endregion

    #region Diagram Pannel

    public void AddOneYaoToDiagramPannel(int cardType)
    {
        diagramPannel.GetComponent<DiagramPanel>().AddOneYao(cardType);
    }

    public void TriggerDiagram(object obj)
    {
        DiagramDataSO diagramData = obj as DiagramDataSO;
        diagramPannel.GetComponent<DiagramPanel>().TriggerDiagram(diagramData, uiFadeDuration);
    }

    #endregion

    #region Mana UI

    public void UpdateManaAmount(int amount)
    {
        TextMeshPro number = manaUI.GetComponentInChildren<TextMeshPro>();
        int currentAmount = int.Parse(number.text);
        if (currentAmount == amount) return;
        number.text = amount.ToString();

        Transform uiTransform = manaUI.transform;
        uiTransform.DOScale(1.1f, uiFadeDuration / 6).SetEase(Ease.OutCubic)
            .OnComplete(() => { uiTransform.DOScale(1f, uiFadeDuration / 6).SetEase(Ease.OutCubic); });
    }

    public void UpdateHasAvailableCard(object obj)
    {
        bool hasAvailable = (bool)obj;
        hasAvailableCard = hasAvailable;
        manaImage.color = new Color(manaImage.color.r, manaImage.color.g, manaImage.color.b, hasAvailable ? 1f : 0.5f);
        manaAmountText.color = new Color(manaAmountText.color.r, manaAmountText.color.g, manaAmountText.color.b,
            hasAvailable ? 1f : 0.5f);
    }

    public void LackOfMana()
    {
        StartCoroutine(LackOfManaCoroutine());
    }

    private IEnumerator LackOfManaCoroutine()
    {
        var dialogText = dialogBox.GetComponentInChildren<TextMeshPro>();
        var dialogBackground = dialogBox.GetComponentInChildren<SpriteRenderer>();
        if (Mathf.Abs(dialogText.color.a - 1f) >= Mathf.Epsilon) yield break; // 如果提示框已经显示，则不重复显示
        // 法力ui变暗
        manaImage.color = new Color(manaImage.color.r, manaImage.color.g, manaImage.color.b, 0.5f);
        manaAmountText.color = new Color(manaAmountText.color.r, manaAmountText.color.g, manaAmountText.color.b, 0.5f);
        // 显示提示框
        dialogBox.SetActive(true);
        dialogText.text = "没有足够法力。";
        yield return new WaitForSeconds(dialogBoxDuration);
        // 淡入淡出
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(dialogBackground.DOFade(0f, 0.5f)).Join(dialogText.DOFade(0f, 0.5f));
        if (hasAvailableCard)
            fadeSequence.Join(manaImage.DOFade(1f, 0.5f)).Join(manaAmountText.DOFade(1f, 0.5f));

        fadeSequence.OnComplete(() =>
        {
            dialogBackground.color = new Color(dialogBackground.color.r, dialogBackground.color.g,
                dialogBackground.color.b, 1f);
            dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, 1f);
            dialogBox.SetActive(false);
        });
    }

    #endregion

    #region Select Diagram Pannel

    public void SelectDiagram()
    {
        selectDiagramPannel.SetActive(true);
    }

    #endregion

    #region Select Card Pannel

    public void SelectAward()
    {
        switch (gameAwardData.awardType)
        {
            case AwardType.Card:
                selectCardPannel.SetActive(true);
                break;
            case AwardType.Blessing:
                //TODO: make select blessing panel active
                break;
        }
    }

    #endregion

    #region Select LevelPannel

    public void SelectLevel()
    {
        selectLevelPannel.SetActive(true);
    }

    #endregion

    public void UpdateMoney(int money)
    {
        moneyUI.text = money.ToString();
    }
}