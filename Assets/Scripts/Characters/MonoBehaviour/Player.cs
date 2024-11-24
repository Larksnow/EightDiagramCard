using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;

    public int CurrentMana {get => playerMana.currentValue; set => playerMana.SetValue(value); }

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
        CurrentMana -= cost;
        if (CurrentMana <= 0) 
        {
            CurrentMana = 0;
        }
    }
}
