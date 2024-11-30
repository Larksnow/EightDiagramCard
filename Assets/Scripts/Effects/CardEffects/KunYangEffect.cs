using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "KunYangEffect", menuName = "Card Effects/Kun Yang Effect")]
public class KunYangEffect : Effect
{
    public DiagramDataSO KunData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        KunData.buffedValue += value;
        Debug.Log($"{triggered.name} was enhanced with Kun Yang.");
    }
}