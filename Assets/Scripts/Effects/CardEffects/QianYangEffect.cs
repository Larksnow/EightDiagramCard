using UnityEngine;

[CreateAssetMenu(fileName = "QianYangEffect", menuName = "Card Effects/Qian Yang Effect")]
public class QianYangEffect : Effect
{
    public DiagramDataSO qianData;
    public CardManager cardManager;
    public CardDataSO qianYangCard;
    public ObjectEventSO activateCardEvent;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        if (triggered == null) return;
        if (cardManager == null) cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        CardDataSO copyData = cardManager.previousCard; // Get the previous card played
        if (copyData == null || copyData == qianYangCard) return;
        Player player = GameObject.FindGameObjectWithTag("player").GetComponent<Player>();
        // Excute the effects of the previous card
        foreach (var effect in copyData.effects)
        {
            effect.Execute(player, triggered, copyData.cardType);
            activateCardEvent.RaiseEvent(copyData, this);
        }
        // Store the copy card as previous card
        cardManager.previousCard = copyData; 
        Debug.Log($"{copyData.name} was copied by Qian Yang.");
    }
}