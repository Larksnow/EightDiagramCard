using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "LiYangEffect", menuName = "Card Effects/Li Yang Effect")]
public class LiYangEffect : Effect
{
    public DiagramDataSO LiData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        LiData.buffedValue += 1;
        Debug.Log($"{triggered.name} was enhanced with Li Yang.");
    }
}