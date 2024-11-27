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

    public void ApplyDiagramEffect(DiagramDataSO diagramData, CardDataSO upYao = null, CardDataSO midYao = null, CardDataSO downYao = null)
    {
        Debug.Log("Applying Diagram Effect");
        if (upYao != null)
        {
            foreach (var effect in upYao.effects)
            {
                // 爻的效果要么直接作用在玩家身上，要么把自己的effect直接给下面的卦
                effect.Execute(player, diagramData);
            }
        }
        if (midYao != null)
        {
            foreach (var effect in midYao.effects)
            {
                effect.Execute(player, diagramData);
            }
        }
        if (downYao != null)
        {
            foreach (var effect in downYao.effects)
            {
                effect.Execute(player, diagramData);
            }
        }
        foreach (var effect in diagramData.effects)
        {
            switch (diagramData.diagramType)
            {
                case DiagramType.Li:// 随机选择单体作为目标
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
                    effect.Execute(enemies[Random.Range(0, enemies.Length)].GetComponent<CharacterBase>(), diagramData);
                    break;
                default:
                    effect.Execute(player, diagramData);
                    break;
            }
        }
    }
}
