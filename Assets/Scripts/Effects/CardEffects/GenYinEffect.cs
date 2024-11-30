using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "GenYinEffect", menuName = "Card Effects/Gen Yin Effect")]
public class GenYinEffect : Effect
{
    public DiagramDataSO GenData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        GenData.buffedValue += 1;
        Debug.Log($"{triggered.name} was enhanced with Gen Yin.");
    }
}