using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Diagram Effects/Draw Card Effect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
    public ObjectEventSO drawYinCardEvent;
    public ObjectEventSO drawYangCardEvent;
    public DiagramDataSO xunData;   // 专属于巽卦
    public int yinBuffCardNumber = 1;
    public int yangBuffCardNumber = 1;

    public struct Draws
    {
        public int amount;
        public CardType cardType;
        public Draws(int amount, CardType cardType) { this.amount = amount; this.cardType = cardType; }
    }

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        drawCardEvent.RaiseEvent(value, this);
        if (xunData.yinBuff)
        {
            Draws draws = new (yinBuffCardNumber, CardType.Yin);
            drawYinCardEvent.RaiseEvent(draws, this);
        }
        if (xunData.yangBuff)
        {
            Draws draws = new (yangBuffCardNumber, CardType.Yang);
            drawYangCardEvent.RaiseEvent(draws, this);
        }
    }
}
