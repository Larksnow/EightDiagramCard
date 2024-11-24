using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//This script controls all the UI elements in the battle scene
public class GamePlayPannel : MonoBehaviour
{
    // 在这里引用各种UI Object
    // 比如 public GameObject cardDeckUI = cardDeck(Prefab) 
    public GameObject cardDeckUI;
    public GameObject discardDeckUI;

    public GameObject endTurnButton;

    public GameObject manaUI;
    
    [Header("Broadcast Events")]
    public ObjectEventSO playerTurnEndEvent;
    // 
    private void OnEnable()
    {
        // 在这里添加你的UI 元素和事件处理程序
    }

    public void OnEndTurnButtonClicked()
    {
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

    public void OnEnemyTurnBegin()
    {
        endTurnButton.GetComponent<EndTurnButton>().pressEnabled = false;
    }

    public void OnPlayerTurnBegin()
    {
        endTurnButton.GetComponent<EndTurnButton>().RotateEndTurnButton();
        endTurnButton.GetComponent<EndTurnButton>().pressEnabled = true;
    }
    
}
