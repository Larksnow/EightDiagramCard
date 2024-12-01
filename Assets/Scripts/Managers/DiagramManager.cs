using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DiagramManager : MonoBehaviour
{
    public List<DiagramDataSO> diagramDataList;
    public Player player;
    // public CharacterBase targetCharacter;
    public ObjectEventSO activateCardEvent;
    public ObjectEventSO triggerDiagramEvent;

    private void Awake()
    {
        InitializeDiagramDataList();
        player = FindObjectOfType<Player>();
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

    public void ApplyDiagramEffect(DiagramDataSO triggered, CardDataSO upYao = null, CardDataSO midYao = null, CardDataSO downYao = null)
    {
        Debug.Log("Applying Diagram Effect");
        // 显示卦象UI文字
        triggerDiagramEvent.RaiseEvent(triggered, this);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        CharacterBase target = null;
        // 先激活所有组成的爻的效果
        if (upYao != null)
        {
            foreach (var effect in upYao.effects)
            {
                // 爻的效果要么直接作用在玩家身上，要么作用在所属的卦
                effect.Execute(player, triggered, upYao.cardType);
                activateCardEvent.RaiseEvent(upYao, this);
            }
        }
        if (midYao != null)
        {
            foreach (var effect in midYao.effects)
            {
                effect.Execute(player, triggered, midYao.cardType);
                activateCardEvent.RaiseEvent(midYao, this);
            }
        }
        if (downYao != null)
        {
            foreach (var effect in downYao.effects)
            {
                effect.Execute(player, triggered, downYao.cardType);
                activateCardEvent.RaiseEvent(downYao, this);
            }
        }
        // 最后触发卦附带的所有效果
        foreach (var effect in triggered.effects) // 目标选择
        {
            if (effect.currentTarget == EffectTargetType.Self)
                effect.Execute(player, triggered);
            else if (effect.currentTarget == EffectTargetType.Single)
            {
                foreach (var enemy in enemies) // 优先攻击嘲讽敌人
                {
                    if (enemy.GetComponent<EnemyBase>().isTaunting)
                    {
                        effect.Execute(enemy.GetComponent<CharacterBase>(), triggered);
                        return;
                    }
                }
                target = enemies[Random.Range(0, enemies.Length)].GetComponent<CharacterBase>();
                effect.Execute(target, triggered);
            }
            else if (effect.currentTarget == EffectTargetType.All)
            {
                foreach (var enemy in enemies)
                {
                    effect.Execute(enemy.GetComponent<CharacterBase>(), triggered);
                }
            }
        }
        // 触发完清空临时buff
        triggered.tempValue = 0;
    }

}
