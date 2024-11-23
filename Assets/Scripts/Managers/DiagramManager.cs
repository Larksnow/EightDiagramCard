using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DiagramManager : MonoBehaviour
{
    public List<DiagramDataSO> diagramDataList;
    public Player player;
    public CharacterBase targetCharacter;

    private void Awake()
    {
        InitializeDiagramDataList();
        player = FindObjectOfType<Player>();
    }
    
    #region Load ALL DiagramDataSO from Addressable
    private void  InitializeDiagramDataList()
    {
        Addressables.LoadAssetsAsync<DiagramDataSO>("DiagramData", null).Completed += OnDiagramDataLoaded;
    }

    private void OnDiagramDataLoaded(AsyncOperationHandle<IList<DiagramDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            diagramDataList = new List<DiagramDataSO>(handle.Result);
        }else{
            Debug.LogError("No Diagram Data Found");
        }
    }
    #endregion

    public void ApplyDiagramEffect(DiagramDataSO diagramData)
    {
        //TODO: ADD a select targetCharacter function using switch case
        foreach (var effect in diagramData.effects)
        {
            effect.Execute(player, player);
        }
    }
    
}
