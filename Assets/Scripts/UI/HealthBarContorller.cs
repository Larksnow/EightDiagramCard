using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarContorller : MonoBehaviour
{
    private CharacterBase currentCharacter;

    [Header("Health Bar")]
    public GameObject fillBarObj;
    public GameObject amountObj;
    private SpriteRenderer fillBar;
    private TextMeshPro amountText;

    private Vector3 originalFilledScale;

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        fillBar = fillBarObj.GetComponent<SpriteRenderer>();
        amountText = amountObj.GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        InitHealthBar();
    }


    [ContextMenu("InitHealthBar")]
    private void InitHealthBar()
    {
        originalFilledScale = fillBar.transform.localScale;
        amountText.text = $"{currentCharacter.MaxHP}/{currentCharacter.MaxHP}";
    }

    // 测试用
    private void Update()
    {
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        if (currentCharacter.isDead)
        {
            fillBar.enabled = false;
            return;
        }
        if (fillBar != null)
        {
            fillBar.enabled = true;
            // 需要保证Sprite的Pivot为(X = 0, Y = 0,5)
            fillBar.transform.localScale = new Vector3((currentCharacter.CurrentHP / (float)currentCharacter.MaxHP) * originalFilledScale.x, originalFilledScale.y, originalFilledScale.z);
            amountText.text = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHP}";
        }
    }
}
