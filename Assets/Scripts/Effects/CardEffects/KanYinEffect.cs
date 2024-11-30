using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "KanYinEffect", menuName = "Card Effects/Kan Yin Effect")]
public class KanYinEffect : Effect
{
    public DiagramDataSO KanData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        KanData.tempValue += value;
        Debug.Log($"{triggered.name} was enhanced with Kan Yin.");
    }
}