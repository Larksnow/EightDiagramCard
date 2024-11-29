using UnityEngine;

[CreateAssetMenu(fileName = "ManaEffect", menuName = "Diagram Effects/Mana Effect")]
public class ManaEffect : Effect
{
    public DiagramDataSO duiData;   // 专属于兑卦
    public int buffValue = 1;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        int manaAmount = value;
        if (duiData.yangBuff)
        {
            manaAmount += buffValue;
        }
        else if (duiData.yinBuff)
        {
            manaAmount += buffValue;
            duiData.yinBuff = false;
        }

        Player player = target as Player;
        if (player == null) Debug.Log("Mana effect can only be applied to player");
        // value 为增加的魔力值
        player.UpdateMana(-manaAmount);
    }
}