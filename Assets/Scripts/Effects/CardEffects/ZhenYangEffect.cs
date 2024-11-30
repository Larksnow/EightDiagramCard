using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "ZhenYangEffect", menuName = "Card Effects/Zhen Yang Effect")]
public class ZhenYangEffect : Effect
{
    public DiagramDataSO ZhenData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        ZhenData.buffedValue += 1;
        Debug.Log($"{triggered.name} was enhanced with Zhen Yang.");
    }
}