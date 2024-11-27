using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
public CardDeck cardDeck;
    public IntVariable playerMana;
    public IntVariable playerMoney;
    public int maxMana;

    public int CurrentMana {get => playerMana.currentValue; set => playerMana.SetValue(value); }
    public int CurrentMoney {get => playerMoney.currentValue; set => playerMoney.SetValue(value); }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue;
    }

    public void NewTrun()
    {
        CurrentMana = maxMana;
    }

    public void UpdateMana(int cost)
    {
        CurrentMana = Mathf.Clamp(CurrentMana - cost, 0, maxMana);
        cardDeck.CheckAllAvailable();
    }
}
