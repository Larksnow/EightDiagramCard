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
    public float dialogBoxDuration;

    private TextMeshPro triggerDiagramText;
    private SpriteRenderer manaImage;
    private TextMeshPro manaAmountText;

    [Header("Broadcast Events")]
    public ObjectEventSO playerTurnEndEvent;
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
        TextMeshPro number = manaUI.GetComponentInChildren<TextMeshPro>();
        number.text = amount.ToString();
    }

    public void LackOfMana()
    {
        StartCoroutine(LackOfManaCoroutine());
    }

    private IEnumerator LackOfManaCoroutine()
    {
        var dialogText = dialogBox.GetComponentInChildren<TextMeshPro>();
        var dialogBackground = dialogBox.GetComponentInChildren<SpriteRenderer>();
        if (dialogText.color.a != 1f) yield break; // 如果提示框已经显示，则不重复显示
        // 法力ui变暗
        manaImage.color = new Color(manaImage.color.r, manaImage.color.g, manaImage.color.b, 0.5f);
        manaAmountText.color = new Color(manaAmountText.color.r, manaAmountText.color.g, manaAmountText.color.b, 0.5f);
        // 显示提示框
        dialogBox.SetActive(true);
        dialogText.text = "没有足够法力。";
        yield return new WaitForSeconds(dialogBoxDuration);
        // 淡入淡出
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(manaImage.DOFade(1f, 0.5f)).Join(manaAmountText.DOFade(1f, 0.5f)).Join(dialogBackground.DOFade(0f, 0.5f)).Join(dialogText.DOFade(0f, 0.5f)).OnComplete(() =>
        {

            dialogBackground.color = new Color(dialogBackground.color.r, dialogBackground.color.g, dialogBackground.color.b, 1f);
            dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, 1f);
            dialogBox.SetActive(false);
        });
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
