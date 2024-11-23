using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList; //Store all cards in the game

    [Header("Card Deck")]
    public CardDeckSO baseCardDeck; //base card deck
    public CardDeckSO playerHoldDeck; //player's card deck
    private void Awake()
    {
        InitializeCardDataList();
        InitializeBaseCardDeck();
    }
    private void InitializeBaseCardDeck()
    {
        foreach (var item in baseCardDeck.CardDeckEntryList)
        {
            playerHoldDeck.CardDeckEntryList.Add(item);
        }
    }
    
    #region Load ALL CardDataSO from Addressable
    private void  InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }else{
            Debug.LogError("No card Data Found");
        }
    }
    #endregion

    public GameObject GetCardFromPool(){
        return poolTool.GetObjectFromPool();
    }

    public void DiscardCard(GameObject card){
        poolTool.ReleaseObjectToPool(card);
    }
}
