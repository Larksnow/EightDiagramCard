using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "LiYinEffect", menuName = "Card Effects/Li Yin Effect")]
public class LiYinEffect : Effect
{
    public DiagramDataSO LiData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        LiData.tempValue += value;
        Debug.Log($"{triggered.name} was enhanced with Li Yin.");
    }
}