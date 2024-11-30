using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "XunYinEffect", menuName = "Card Effects/Xun Yin Effect")]
public class XunYinEffect : Effect
{
    public DiagramDataSO XunData;
    public DrawCardEffect drawCardEffect;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        drawCardEffect.specialCardsToDraw.Add(CardType.Yin, 1);
        Debug.Log($"{triggered.name} was enhanced with Xun Yin.");
    }
}