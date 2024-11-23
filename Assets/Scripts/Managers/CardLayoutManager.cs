using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    public Vector3 centerPoint;
    
    [SerializeField] private List<Vector3> cardPositions = new();
    [SerializeField] private List<Quaternion> cardRotations = new();

    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculatePositions(totalCards, isHorizontal);
        return new CardTransform(cardPositions[index], cardRotations[index]);
    }

    private void CalculatePositions(int cardNumber, bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        if(horizontal)
        {
            float currentWidth = cardSpacing * (cardNumber - 1);
            float totalWith = Mathf.Min(maxWidth, currentWidth);

            float currentCardSpacing = totalWith > 0 ? totalWith / (cardNumber - 1) : 0;

            for (int i = 0; i < cardNumber; i++)
            {
                float xPos = 0 - (totalWith / 2) + (i * currentCardSpacing);
                var pos = new Vector3(xPos, centerPoint.y, 0);
                cardPositions.Add(pos);
                var rotation = Quaternion.identity;
                cardRotations.Add(rotation);
            }
        }
    }

}
