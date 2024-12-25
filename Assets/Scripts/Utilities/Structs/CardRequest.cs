using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct CardRequest
{
    public int amount;
    public CardType cardType;
    public CardRequest(int amount, CardType cardType) { this.amount = amount; this.cardType = cardType; }
}