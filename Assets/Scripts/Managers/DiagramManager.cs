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

    public void ApplyDiagramEffect(DiagramDataSO diagramData, CardDataSO upYao, CardDataSO midYao, CardDataSO downYao)
    {
        // TODO：首先触发所有组成此卦的爻的Effect， up, mid, down 顺序
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
            //思考一下在哪里，由谁去选取正确的对象，可以由本脚本通过switch cases来选取（所有八卦的作用对象，甚至还要写随机选取），也可以由外部传入一个target
            effect.Execute(player);
        }
    }
    
}
