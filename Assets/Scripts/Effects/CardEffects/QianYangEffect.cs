using UnityEngine;

[CreateAssetMenu(fileName = "CopyCardEffect", menuName = "Card Effects/Copy Card Effect")]
public class CopyCardEffect : CardEffect
{
    public CardManager cardManager;
    public CardDataSO copyData;
    public ObjectEventSO activateCardEvent;

    public override void Execute(DiagramDataSO triggeredDiagram)
    {
        if (cardManager == null) cardManager = FindObjectOfType<CardManager>();
        copyData = cardManager.previousCard; // Get the previous card played
        if (copyData == null || copyData.effects.Contains(this)) return;
        Player player = GameObject.FindGameObjectWithTag("player").GetComponent<Player>();
        // Excute the effects of the previous card
        foreach (var effect in copyData.effects)
        {
            effect.Execute(triggeredDiagram);
            activateCardEvent.RaiseEvent(copyData, this);
        }
        // Store the copy card as previous card
        cardManager.previousCard = copyData; 
        Debug.Log($"{copyData.name} was copied by Qian Yang.");
    }
}