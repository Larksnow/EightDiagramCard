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
        // TODO: buff更新（现在在PlayerTurnBegin事件中更新)
    }

    public override void OnTurnBegin()
    {
        base.OnTurnBegin();
        CurrentMana = maxMana;
    }

    public void UpdateMana(int cost)
    {
        CurrentMana = Mathf.Clamp(CurrentMana - cost, 0, maxMana);
        cardDeck.CheckAllCardsState();
    }
}
