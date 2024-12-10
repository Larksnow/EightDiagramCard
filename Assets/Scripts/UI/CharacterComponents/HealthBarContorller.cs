using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class HealthBarContorller : MonoBehaviour
{
    public CharacterBase currentCharacter;
    public float gradientDuration;
    
    [Header("Health Bar")] public SpriteRenderer fillBar;
    public SpriteRenderer gradientBar;
    public TextMeshPro amountText;

    private Vector3 originalFilledScale;

    private void Awake()
    {
        currentCharacter = GetComponentInParent<CharacterBase>();
    }

    private void Start()
    {
        InitHealthBar();
    }


    [ContextMenu("InitHealthBar")]
    private void InitHealthBar()
    {
        originalFilledScale = fillBar.transform.localScale;
        amountText.text = $"{currentCharacter.maxHP}/{currentCharacter.maxHP}";
    }

    #region Event Listening

    public void UpdateHealth(object obj)
    {
        HPChange hPChange = (HPChange)obj;
        if (hPChange.target != currentCharacter) return;

        int currentHealth = hPChange.updated;

        // [需要保证fillBar Sprite的Pivot为(X = 0, Y = 0,5)]
        fillBar.transform.localScale =
            new Vector3(currentHealth / (float)currentCharacter.maxHP * originalFilledScale.x,
                originalFilledScale.y, originalFilledScale.z);
        amountText.text = $"{currentHealth}/{currentCharacter.maxHP}";

        // 血条渐变效果
        Sequence gradientSequence = DOTween.Sequence();
        gradientSequence.Append(gradientBar.transform.DOScale(new Vector3(
                currentHealth / (float)currentCharacter.maxHP * originalFilledScale.x,
                originalFilledScale.y, originalFilledScale.z), gradientDuration))
            .SetEase(Ease.OutSine);
    }

    #endregion
}