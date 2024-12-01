using System.Collections.Generic;
using UnityEngine;

public class CardListPanelController : MonoBehaviour
{
    public GameObject CardListPanel;

    private CardDeck cardDeck;
    private CardManager cardManager;
    private Vector3 beginPos;
    private List<CardDeckEntry> cardList;
    private bool isDisplaying = false;

    private void Awake()
    {
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        cardDeck = GameObject.Find("CardDeck").GetComponent<CardDeck>();
        beginPos = transform.position;
    }

    public void ToggleCardListPanel(CardListType cardListType)
    {
        isDisplaying = !isDisplaying;
        if (isDisplaying)
        {
            switch (cardListType)
            {
                case CardListType.PlayerHold:
                    cardList = cardManager.playerHoldDeck.CardDeckEntryList;
                    break;
                case CardListType.DrawDeck:
                    cardList = cardDeck.GetDrawDeck();
                    break;
                case CardListType.DiscardDeck:
                    cardList = cardDeck.GetDiscardDeck();
                    break;
            }
        }
    }
}
