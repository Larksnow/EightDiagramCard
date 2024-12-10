using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CopyCardEffect", menuName = "Card Effects/Copy Card Effect")]
public class CopyCardEffect : CardEffect
{
    public CardDataSO copyData;
    public ObjectEventSO displayCardNameEvent;

    public override void Execute(DiagramDataSO triggeredDiagram)
    {
    }

    public void ExecuteWithIndex(int index, List<CardDataSO> cardList, DiagramDataSO triggeredDiagram)
    {
        copyData = cardList.Count - 1 < index ? null : cardList[index + 1];
        if (copyData == null || copyData.effects == null || copyData.effects.Count == 0)
        {
            Debug.Log("Nothing to copy");
            return;
        } 
        foreach (var effect in copyData.effects)
        {
            effect.Execute(triggeredDiagram);
            displayCardNameEvent.RaiseEvent(copyData, this);
        }
        Debug.Log("Copied effect: " + copyData.cardName);
    }
}