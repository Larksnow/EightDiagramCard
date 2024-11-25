using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
//This script controls all the UI elements in the battle scene
public class GamePlayPannel : MonoBehaviour
{
    // 在这里引用各种UI Object
    // 比如 public GameObject cardDeckUI = cardDeck(Prefab) 
    public GameObject cardDeckUI;
    public GameObject discardDeckUI;

    public GameObject endTurnButton;

    public GameObject manaUI;
    public GameObject diagramPannel;
    public GameObject dialogBox;
    public float uiFadeDuration;

    private TextMeshPro triggerDiagramText;
    private int lastManaAmount;
    private SpriteRenderer manaImage;
    private TextMeshPro manaAmountText;

    [Header("Broadcast Events")]
    public ObjectEventSO playerTurnEndEvent;
    // 
    private void OnEnable()
    {
        triggerDiagramText = diagramPannel.GetComponentInChildren<TextMeshPro>();
        manaImage = manaUI.GetComponentInChildren<SpriteRenderer>();
        manaAmountText = manaUI.GetComponentInChildren<TextMeshPro>();
    }

    public void OnEndTurnButtonClicked()
    {
        // Rest DiagramePannel When player turn ended
        diagramPannel.GetComponent<DiagramPannel>().ResetDiagramPannel();
        playerTurnEndEvent.RaiseEvent(null, this);
    }

    public void UpdateDrawDeckAmount(int amount)
    {
        TextMeshPro number = cardDeckUI.GetComponentInChildren<TextMeshPro>();
        number.text = amount.ToString();
    }

    public void UpdateDiscardDeckAmount(int amount)
    {
        TextMeshPro number = discardDeckUI.GetComponentInChildren<TextMeshPro>();
        number.text = amount.ToString();
    }
    public void UpdateManaAmount(int amount)
    {
        if (lastManaAmount != 0 && amount == 0) UseUpManaUI();
        if (lastManaAmount == 0 && amount != 0) RecoverManaUI();
        TextMeshPro number = manaUI.GetComponentInChildren<TextMeshPro>();
        number.text = amount.ToString();
        lastManaAmount = amount;
    }

    private void UseUpManaUI()
    {
        manaImage.color = new Color(manaImage.color.r, manaImage.color.g, manaImage.color.b, 0.5f);
        manaAmountText.color = new Color(manaAmountText.color.r, manaAmountText.color.g, manaAmountText.color.b, 0.5f);
        dialogBox.gameObject.SetActive(true);
        dialogBox.GetComponentInChildren<TextMeshPro>().text = "没有足够法力。";
    }

    private void RecoverManaUI()
    {
        manaImage.color = new Color(manaImage.color.r, manaImage.color.g, manaImage.color.b, 1f);
        manaAmountText.color = new Color(manaAmountText.color.r, manaAmountText.color.g, manaAmountText.color.b, 1f);
        dialogBox.gameObject.SetActive(false);
        dialogBox.GetComponentInChildren<TextMeshPro>().text = "";
    }

    public void OnEnemyTurnBegin()
    {
        endTurnButton.GetComponent<EndTurnButton>().pressEnabled = false;
    }

    public void OnPlayerTurnBegin()
    {
        endTurnButton.GetComponent<EndTurnButton>().RotateEndTurnButton();
        endTurnButton.GetComponent<EndTurnButton>().pressEnabled = true;
    }

    #region Diagram Pannel
    public void AddOneYaoToDiagramPannel(int cardType)
    {
        diagramPannel.GetComponent<DiagramPannel>().AddOneYao(cardType);
    }
    public void TriggerDiagram(object obj)
    {
        string diagramName = obj as string;
        Debug.Log("Trigger Diagram: " + diagramName);
        diagramPannel.GetComponent<DiagramPannel>().HighlightTop3();
        triggerDiagramText.text = diagramName;
        triggerDiagramText.color = new Color(triggerDiagramText.color.r, triggerDiagramText.color.g, triggerDiagramText.color.b, 1f);

        Sequence textAnimationSequence = DOTween.Sequence();
        textAnimationSequence.Append(triggerDiagramText.DOFade(0f, uiFadeDuration)).onComplete = () =>
        {
            triggerDiagramText.text = "";
        };
    }
    #endregion
}
