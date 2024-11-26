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

    public void ApplyDiagramEffect(DiagramDataSO diagramData, CardDataSO upYao, CardDataSO midYao, CardDataSO downYao)
    {
        // TODO:首先触发所有组成此卦的爻的Effect， up, mid, down 顺序
        foreach (var effect in upYao.effects)
        {
            // 爻的效果要么直接作用在玩家身上，要么把自己的effect直接给下面的卦
            // effect.Execute();
        }
        foreach (var effect in midYao.effects)
        {
            // 爻的效果要么直接作用在玩家身上，要么把自己的effect直接给下面的卦
            // effect.Execute();
        }
        foreach (var effect in downYao.effects)
        {
            // 爻的效果要么直接作用在玩家身上，要么把自己的effect直接给下面的卦
            // effect.Execute();
        }
        //Pending: ADD a select targetCharacter function using switch case
        foreach (var effect in diagramData.effects)
        {
            CharacterBase target = GetDiagramTargets(diagramData.diagramType);
            effect.Execute(target);
        }
    }

    private CharacterBase GetDiagramTargets(DiagramType diagramType)
    {
        CharacterBase target;
        switch (diagramType)
        {
            case DiagramType.Li:// 随机选择单体作为目标
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
                target = enemies[Random.Range(0, enemies.Length)].GetComponent<CharacterBase>();
                break;
            default:
                target = player;
                break;
        }
        return target;
    }

}
