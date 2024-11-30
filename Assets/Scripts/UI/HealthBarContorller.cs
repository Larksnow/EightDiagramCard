using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarContorller : MonoBehaviour
{
    public CharacterBase currentCharacter;

    [Header("Health Bar")]
    public GameObject fillBarObj;
    public GameObject amountObj;
    private SpriteRenderer fillBar;
    private TextMeshPro amountText;

    private Vector3 originalFilledScale;

    private void Awake()
    {
        fillBar = fillBarObj.GetComponent<SpriteRenderer>();
        amountText = amountObj.GetComponent<TextMeshPro>();
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

    public void UpdateHealth(object obj)
    {
        CharacterBase.HPChange hPChange= (CharacterBase.HPChange)obj;
        if (hPChange.target != currentCharacter) return;

        int currentHealth = hPChange.updated;
        if (currentCharacter.isDead)
        {
            fillBar.enabled = false;
            return;
        }
        if (fillBar != null)
        {
            fillBar.enabled = true;
            // 需要保证Sprite的Pivot为(X = 0, Y = 0,5)
            fillBar.transform.localScale = new Vector3(currentHealth / (float)currentCharacter.maxHP * originalFilledScale.x, originalFilledScale.y, originalFilledScale.z);
            amountText.text = $"{currentHealth}/{currentCharacter.maxHP}";
        }
    }
}
