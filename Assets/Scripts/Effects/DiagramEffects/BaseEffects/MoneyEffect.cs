using UnityEngine;


[CreateAssetMenu(fileName = "MoneyEffect", menuName = "Diagram Effects/Money Effect")]

public class MoneyEffect : DiagramEffect
{
    public override void Execute(CharacterBase target)
    {
        Player player = target as Player;
        if (player == null)
            Debug.Log("Money effect can only be applied to player characters.");
        player.UpdateMoney(value);
    }
}