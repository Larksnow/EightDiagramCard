using UnityEngine;


[CreateAssetMenu(fileName = "MoneyEffect", menuName = "Diagram Effects/Money Effect")]

public class MoneyEffect : Effect
{
    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        Player player = target as Player;
        if (player == null)
            Debug.Log("Money effect can only be applied to player characters.");
        player.CurrentMoney += value;
    }
}