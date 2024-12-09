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
    public CardDataSO previousCard; //Store the previous card

    [Header("Card Deck")]
    public CardDeckSO baseCardDeck; //base card deck
    public CardDeckSO playerHoldDeck; //player's card deck
    public ObjectEventSO playYangCard;
    public ObjectEventSO playYinCard;
    private void Awake()
    {
        InitializeCardDataList();
        InitializeBaseCardDeck();
    }

    private void OnDisable()
    {
        //Make sure card is disbale after game shut down
        playerHoldDeck.CardDeckEntryList.Clear();
        previousCard = null;
    }
    private void InitializeBaseCardDeck()
    {
        foreach (var item in baseCardDeck.CardDeckEntryList)
        {
            playerHoldDeck.CardDeckEntryList.Add(item);
        }
    }

    #region Load ALL CardDataSO from Addressable
    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No card Data Found");
        }
    }
    #endregion

    // When draw a card, get the card Game Object from pool
    public GameObject GetCardFromPool()
    {
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }

    public void DiscardCard(GameObject card)
    {
        poolTool.ReleaseObjectToPool(card);
    }

    public void UpdatePreviousCard(object obj)
    {
        Card card = obj as Card;
        if (card.cardData.cardType == CardType.Yang)
        {
            playYangCard.RaiseEvent(null, this);
        }else if(card.cardData.cardType == CardType.Yin)
        {
            playYinCard.RaiseEvent(null, this);
        }
        previousCard = card.cardData;
    }

    public void AddCardToPlayerHoldDeck(CardDataSO cardData, int amount)
    {
        playerHoldDeck.AddCard(cardData, amount);
    }
}
