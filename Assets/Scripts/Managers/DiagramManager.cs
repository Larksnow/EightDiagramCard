using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DiagramManager : MonoBehaviour
{
    public List<DiagramDataSO> diagramDataList;
    public Player player;
    private GameManager gameManager;

    [Header("Broadcast Events")]
    public ObjectEventSO displayCardNameEvent;
    public ObjectEventSO triggerDiagramEvent;

    private void Awake()
    {
        InitializeDiagramDataList();
        gameManager = FindObjectOfType<GameManager>();
    }

    #region Load ALL DiagramDataSO from Addressable
    private void InitializeDiagramDataList()
    {
        Addressables.LoadAssetsAsync<DiagramDataSO>("DiagramData", null).Completed += OnDiagramDataLoaded;
    }

    private void OnDiagramDataLoaded(AsyncOperationHandle<IList<DiagramDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            diagramDataList = new List<DiagramDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No Diagram Data Found");
        }
    }
    #endregion

    public void ApplyCardsEffect(List<CardDataSO> cardList, DiagramDataSO triggeredDiagram)
    {
        for (int i = 0; i < 3; i++) // Execute First Three cards in the list
        {
            var card = cardList[i];  // Get the card at index i
            displayCardNameEvent.RaiseEvent(card, this);  // Raise the event to display the card name
            foreach (var effect in card.effects)
            {
                if (effect is CopyCardEffect copyEffect)
                {
                    copyEffect.ExecuteWithIndex(i, cardList, triggeredDiagram);
                }
                else {
                    effect.Execute(triggeredDiagram);  // Execute each effect
                }
            }
        }
    }

    public void ApplyDiagramEffect(DiagramDataSO triggeredDiagram)
    {
        Debug.Log("Applying Diagram Effect: " + triggeredDiagram);
        // 显示卦象UI文字
        triggerDiagramEvent.RaiseEvent(triggeredDiagram, this);
        // 最后触发所有diagram effect
        for (int i = 0; i < triggeredDiagram.triggerTime; i++)
        {
            foreach (var effect in triggeredDiagram.effects) // 目标选择
            {
                Debug.Log("##############" + effect.GetFormattedDescription());
                switch (effect.currentTargetType)
                {
                    case EffectTargetType.Self:
                        effect.Execute(player);
                        break;
                    case EffectTargetType.All:
                        foreach (var enemy in gameManager.enemyList)
                            effect.Execute(enemy.GetComponent<CharacterBase>());
                        break;
                    case EffectTargetType.Random:
                        foreach (var enemy in gameManager.enemyList)
                        {
                            if (enemy.GetComponent<EnemyBase>().isTaunting){
                                effect.Execute(enemy.GetComponent<CharacterBase>());
                                break;
                            }
                        }
                        effect.Execute(gameManager.enemyList[Random.Range(0, gameManager.enemyList.Count)].GetComponent<CharacterBase>());
                        break;
                    case EffectTargetType.Lowest:
                        CharacterBase target = null;
                        foreach (var enemy in gameManager.enemyList)
                        {
                            if (enemy.GetComponent<EnemyBase>().isTaunting)
                            {
                                effect.Execute(enemy.GetComponent<CharacterBase>());
                                break;
                            }
                            // Find the enemy with lowest HP
                            if (target == null || enemy.GetComponent<CharacterBase>().currentHP < target.currentHP)
                                target = enemy.GetComponent<CharacterBase>();
                        }
                        effect.Execute(target);
                        break;
                }
            }
        }
        triggeredDiagram.ResetAfterTrigger();
    }

    public void ResetDiagramAfterBattle()
    {
        foreach (var diagram in diagramDataList)
        {
            diagram.ResetAfterBattle();
        }
    }

}
