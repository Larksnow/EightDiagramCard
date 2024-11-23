using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float maxAngle = 21f;
    public float cardSpacing = 2f;
    public Vector3 centerPoint;
    [Header("Cricle")]
    public float radius = 17f;
    public float angleBetweenCards = 7f;

    [SerializeField] private List<Vector3> cardPositions = new();
    [SerializeField] private List<Quaternion> cardRotations = new();
    

    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -5.3f : Vector3.up * -30f;
    }
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
        else
        {
            float cardAngle = (cardNumber - 1) * angleBetweenCards / 2;
            Debug.Log("angleBetweenCards: " + angleBetweenCards);
            float totalAngle = Mathf.Min(maxAngle, cardAngle);
            float currentCardAngle = totalAngle > 0 ? 2 * totalAngle / (cardNumber - 1) : 0;
            Debug.Log("currentCardAngle: " + currentCardAngle);
            for (int i = 0; i < cardNumber; i++)
            {
                var pos = FanCardPosition(totalAngle - i * currentCardAngle);
                var rotation = Quaternion.Euler(0, 0, totalAngle - i * currentCardAngle);
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - MathF.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + MathF.Cos(Mathf.Deg2Rad * angle) * radius,
            z:0
        );
    }

}
