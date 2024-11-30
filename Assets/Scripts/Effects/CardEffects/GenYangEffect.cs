using UnityEngine;

// 乾卦阳/阴符buff效果
[CreateAssetMenu(fileName = "GenYangEffect", menuName = "Card Effects/Gen Yang Effect")]
public class GenYangEffect : Effect
{
    public DiagramDataSO GenData;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        GenData.tempValue += value;
        Debug.Log($"{triggered.name} was enhanced with Gen Yang.");
    }
}