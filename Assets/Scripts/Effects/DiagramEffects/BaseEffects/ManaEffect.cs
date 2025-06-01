using UnityEngine;

[CreateAssetMenu(fileName = "ManaEffect", menuName = "Diagram Effects/Mana Effect")]
public class ManaEffect : DiagramEffect
{
    public bool unlimit = false;
    public override void Execute(CharacterBase target)
    {
        Player player = target as Player;
        if (player == null) Debug.Log("Mana effect can only be applied to player");
        // value 为增加的魔力值
        int currentMana = player.currentMana;
        player.UpdateMana(value);
    }
}