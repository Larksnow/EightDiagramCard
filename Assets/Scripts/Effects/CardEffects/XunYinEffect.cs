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
        if (drawCardEffect.specialCardsToDraw.TryGetValue(CardType.Yin, out int currentValue))
        {
            drawCardEffect.specialCardsToDraw[CardType.Yin] = currentValue + 1;
        }
        else
        {
            drawCardEffect.specialCardsToDraw.Add(CardType.Yin, value);  // Add if the key does not exist
        }
        Debug.Log($"{triggered.name} was enhanced with Xun Yin.");
    }
}