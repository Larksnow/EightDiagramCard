using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyIntentionUI : MonoBehaviour
{
    public EnemyBase currentEnemy;
    public GameObject effectIconPrefab;
    public Vector3 centerPosition = new(0, 0, 0);
    public float spacing = 0.2f;

    private FadeInOutHandler fadeInOutHandler;
    private List<EnemyEffect> nextTurnEffects = new();
    private float iconWidth;
    private bool isFadeOutComplete = false;

    private void Awake()
    {
        iconWidth = effectIconPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        fadeInOutHandler = GetComponent<FadeInOutHandler>();
    }

    #region Event Listening

    // EnemyTurnBegin时清除意图
    public void ClearIntents()
    {
        StartCoroutine(ClearIntentsCoroutine());
    }

    // PlayerTurnBegin时需要显示意图
    public void ShowIntents()
    {
        StartCoroutine(ShowIntentsCoroutine());
    }

    // Switch Day/Night时需要切换意图，清除旧的意图和显示新的意图需连续进行
    public void ChangeIntents()
    {
        StartCoroutine(ChangeIntentsCoroutine());
    }

    #endregion

    private IEnumerator ChangeIntentsCoroutine()
    {
        yield return StartCoroutine(ClearIntentsCoroutine());
        yield return StartCoroutine(ShowIntentsCoroutine());
    }

    private IEnumerator ShowIntentsCoroutine()
    {
        nextTurnEffects = currentEnemy.GetNextTurnEffects();
        var numEffects = nextTurnEffects.Count;
        if (numEffects == 0) yield break;


        // 设置左右对称的分布
        for (var i = 0; i < numEffects; i++)
        {
            // 计算当前意图的位置
            var offset = (i - (numEffects - 1) / 2f) * (iconWidth + spacing);
            var iconPosition = centerPosition + new Vector3(offset, 0, 0);
            var nextEffect = nextTurnEffects[i];

            // 创建意图图标
            var icon = Instantiate(effectIconPrefab, iconPosition, Quaternion.identity);
            icon.GetComponent<SpriteRenderer>().sprite = nextEffect.icon;

            var amountText = icon.transform.Find("Amount").GetComponent<TextMeshPro>();
            var timesText = icon.transform.Find("Times").GetComponent<TextMeshPro>();
            timesText.enabled = false;

            switch (nextEffect.effectType)
            {
                case EnemyEffectType.Damage:
                    var damageEffect = (EnemyDamageEffect)nextEffect;

                    amountText.text = nextEffect.Value.ToString();
                    // 大于1次的攻击显示次数
                    if (damageEffect.damageTimes > 1)
                    {
                        timesText.enabled = true;
                        timesText.text = "x" + damageEffect.damageTimes;
                    }

                    break;
                case EnemyEffectType.Shield:
                    amountText.text = nextEffect.Value.ToString();
                    break;
                default:
                    // 非数值型意图(除了攻击、防御)不显示数值
                    amountText.enabled = false;
                    break;
            }

            icon.transform.SetParent(transform, false);
        }

        fadeInOutHandler.UpdateUIElements();
        fadeInOutHandler.FadeIn();
    }

    private IEnumerator ClearIntentsCoroutine()
    {
        isFadeOutComplete = false;
        fadeInOutHandler.FadeOut(() => { StartCoroutine(DestroyChildren()); });
        yield return new WaitUntil(() => isFadeOutComplete);
    }

    private IEnumerator DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        yield return null; // 等待下一帧子物体被销毁后再设置标记为true

        isFadeOutComplete = true;
    }
}