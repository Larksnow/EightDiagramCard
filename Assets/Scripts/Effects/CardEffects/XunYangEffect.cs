using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "XunYangEffect", menuName = "Card Effects/Xun Yang Effect")]
public class XunYangEffect : Effect
{
    public DiagramDataSO XunData;
    public DrawCardEffect drawCardEffect;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        drawCardEffect.specialCardsToDraw.Add(CardType.Yang, value);
        Debug.Log($"{triggered.name} was enhanced with Xun Yang.");
    }
}