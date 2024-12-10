using UnityEngine;

public class Player : CharacterBase
{
public CardDeck cardDeck;
    public int maxMana;
    public int drawCountEachTurn = 5;

    [Header("Broadcast Events")]
    public IntEventSO updateManaEvent;
    public IntEventSO updateMoneyEvent;


    public int currentMana; 
    public int currentMoney; 

    private void OnEnable()
    {
        currentMana = maxMana;
    }

    public override void OnTurnBegin()
    {
        base.OnTurnBegin();
        UpdateMana(maxMana);
    }

    public void UpdateMana(int amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        cardDeck.CheckAllCardsState();
        updateManaEvent.RaiseEvent(currentMana, this);
    }

    public void UpdateMoney(int amount)
    {
        currentMoney += amount;
        updateMoneyEvent.RaiseEvent(currentMoney, this);
    }
}
