using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "ZhenYinEffect", menuName = "Card Effects/Zhen Yin Effect")]
public class ZhenYinEffect : Effect
{
    public DiagramDataSO ZhenData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        //TODO: add a trigger numbers in diagramdataso
        Debug.Log($"{triggered.name} was enhanced with Zhen Yin.");
    }
}