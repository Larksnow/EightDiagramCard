using UnityEngine;

[CreateAssetMenu(fileName = "ManaEffect", menuName = "Diagram Effects/Mana Effect")]
public class ManaEffect : Effect
{
    public DiagramDataSO diagramData;   // 专属于兑卦
    public bool unlimit = false;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        int manaAmount = diagramData.basicValue + diagramData.buffedValue + diagramData.tempValue;
        Player player = target as Player;
        if (player == null) Debug.Log("Mana effect can only be applied to player");
        // value 为增加的魔力值
        int currentMana = player.currentMana;
        if (currentMana + manaAmount > player.maxMana && !unlimit) // 未解禁时法力无法超过上限
        {
            manaAmount = player.maxMana - currentMana;
        }
        player.UpdateMana(manaAmount);
    }
}