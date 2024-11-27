using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
//This script controls all the UI elements in the battle scene
public class GamePlayPanel : MonoBehaviour
{
    // 在这里引用各种UI Object
    // 比如 public GameObject cardDeckUI = cardDeck(Prefab) 
    [Header("UI Objects")]
    public GameObject cardDeckUI;
    public GameObject discardDeckUI;
    public GameObject endTurnButton;
    public GameObject manaUI;
    public GameObject diagramPannel;
    public GameObject dialogBox;
    public GameObject selectDiagramPannel;
    public GameObject damageNumberUI;

    public float uiFadeDuration;
    public float dialogBoxDuration;

    private SpriteRenderer manaImage;
    private TextMeshPro manaAmountText;
    private bool hasAvailableCard;

    [Header("Broadcast Events")]
    public ObjectEventSO playerTurnEndEvent;
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
        playerTurnEndEvent.RaiseEvent(null, this);
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
    #endregion

    #region Card Deck UI
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
        number.text = amount.ToString();
    }
    public void UpdateHasAvailableCard(object obj)
    {
        bool hasAvailable = (bool)obj;
        hasAvailableCard = hasAvailable;
        manaImage.color = new Color(manaImage.color.r, manaImage.color.g, manaImage.color.b, hasAvailable ? 1f : 0.5f);
        manaAmountText.color = new Color(manaAmountText.color.r, manaAmountText.color.g, manaAmountText.color.b, hasAvailable ? 1f : 0.5f);
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
        fadeSequence.Append(dialogBackground.DOFade(0f, 0.5f)).Join(dialogText.DOFade(0f, 0.5f));
        if (hasAvailableCard)
            fadeSequence.Join(manaImage.DOFade(1f, 0.5f)).Join(manaAmountText.DOFade(1f, 0.5f));

        fadeSequence.OnComplete(() =>
        {
            dialogBackground.color = new Color(dialogBackground.color.r, dialogBackground.color.g, dialogBackground.color.b, 1f);
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

    #region 
    public void ShowDamageNumber(object obj)
    {
        float diminishDuration = 0.5f;
        float descentDuration = 1f;
        int descentHeight = 5;
        CharacterBase.Damage damage = (CharacterBase.Damage)obj;
        TextMeshPro damageNumber = damageNumberUI.GetComponent<TextMeshPro>();
        Color originalColor = damageNumber.color;
        Vector3 originPosition = damageNumberUI.transform.position;
        Vector3 originScale = damageNumberUI.transform.localScale;

        damageNumberUI.SetActive(true);
        damageNumberUI.transform.position = damage.position;
        damageNumber.text = damage.amount.ToString();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(damageNumberUI.transform.DOScale(damageNumberUI.transform.localScale * 0.4f, diminishDuration))
        .Join(damageNumber.DOColor(Color.white, diminishDuration))
        .Join(damageNumberUI.transform.DOMove(damageNumberUI.transform.position + new Vector3(1, -0.5f, 0), diminishDuration))
        .Append(damageNumberUI.transform.DOMove(new Vector3(damageNumberUI.transform.position.x + 1, damageNumberUI.transform.position.y - descentHeight, damageNumberUI.transform.position.z), descentDuration))
        .Join(damageNumber.DOFade(0f, descentDuration))
        .OnComplete(() =>
        {
            damageNumberUI.SetActive(false);
            damageNumber.color = originalColor;
            damageNumberUI.transform.localScale = originScale;
            damageNumberUI.transform.position = originPosition;
        });
    }
    #endregion
}
