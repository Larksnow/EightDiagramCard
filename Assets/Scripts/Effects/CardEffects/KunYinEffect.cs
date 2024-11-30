using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "KunYinEffect", menuName = "Card Effects/Kun Yin Effect")]
public class KunYinEffect : Effect
{
    public DiagramDataSO KunData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        KunData.tempValue += value;
        Debug.Log($"{triggered.name} was enhanced with Kun Yin.");
    }
}