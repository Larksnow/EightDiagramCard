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

    public void ApplyDiagramEffect(DiagramDataSO triggered, CardDataSO upYao = null, CardDataSO midYao = null, CardDataSO downYao = null)
    {
        Debug.Log("Applying Diagram Effect");
        if (upYao != null)
        {
            foreach (var effect in upYao.effects)
            {
                // 爻的效果要么直接作用在玩家身上，要么把自己的effect直接给下面的卦
                effect.Execute(player, triggered, upYao.cardType);
            }
        }
        if (midYao != null)
        {
            foreach (var effect in midYao.effects)
            {
                effect.Execute(player, triggered, midYao.cardType);
            }
        }
        if (downYao != null)
        {
            foreach (var effect in downYao.effects)
            {
                effect.Execute(player, triggered, downYao.cardType);
            }
        }
        foreach (var effect in triggered.effects)
        {
            effect.Execute(player, triggered);
        }
    }
}
