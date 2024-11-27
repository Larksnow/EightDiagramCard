using UnityEngine;


[CreateAssetMenu(fileName = "MoneyEffect", menuName = "Diagram Effects/Money Effect")]

public class MoneyEffect : Effect
{
    public override void Execute(CharacterBase target)
    {
        Player player = target as Player;
        if (player == null) 
            Debug.Log("Money effect can only be applied to player characters.");
        player.CurrentMoney += value;
    }

    public override void Execute(DiagramDataSO diagramData)
    {
        throw new System.NotImplementedException();
    }
}