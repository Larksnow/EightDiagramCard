using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "DuiYangEffect", menuName = "Card Effects/Dui Yang Effect")]
public class DuiYangEffect : Effect
{
    public DiagramDataSO DuiData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        DuiData.tempValue += value;
        Debug.Log($"{triggered.name} was enhanced with Dui Yang.");
    }
}