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
        if (drawCardEffect.specialCardsToDraw.TryGetValue(CardType.Yang, out int currentValue))
        {
            drawCardEffect.specialCardsToDraw[CardType.Yang] = currentValue + 1;
        }
        else
        {
            drawCardEffect.specialCardsToDraw.Add(CardType.Yang, value);  // Add if the key does not exist
        }
        Debug.Log($"{triggered.name} was enhanced with Xun Yang.");
    }
}