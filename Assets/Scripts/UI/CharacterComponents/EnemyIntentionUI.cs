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

    private void Awake()
    {
        iconWidth = effectIconPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        fadeInOutHandler = GetComponent<FadeInOutHandler>();
    }

    #region Event Listening

    public void ShowUI()
    {
        StartCoroutine(ShowUICoroutine());
    }

    #endregion

    private IEnumerator ShowUICoroutine()
    {
        yield return ClearUICoroutine();

        nextTurnEffects = currentEnemy.GetNextTurnEffects();
        var numEffects = nextTurnEffects.Count;
        if (numEffects == 0) yield break;

        // 设置左右对称的分布
        for (var i = 0; i < numEffects; i++)
        {
            // 计算当前效果的位置
            var offset = (i - (numEffects - 1) / 2f) * (iconWidth + spacing);
            var iconPosition = centerPosition + new Vector3(offset, 0, 0);
            var nextEffect = nextTurnEffects[i];

            // 创建图标
            var icon = Instantiate(effectIconPrefab, iconPosition, Quaternion.identity);
            icon.GetComponent<SpriteRenderer>().sprite = nextEffect.icon;
            var text = icon.GetComponentInChildren<TextMeshPro>();
            if (nextEffect.effectType is EnemyEffectType.Damage or EnemyEffectType.Shield)
            {
                text.text = nextEffect.GetRuntimeValue().ToString();
            }
            else
            {
                // 非数值型意图(除了攻击、防御)不显示数值
                text.enabled = false;
            }

            icon.transform.SetParent(transform, false);
        }

        fadeInOutHandler.FadeIn();
    }

    private IEnumerator ClearUICoroutine()
    {
        bool isFadeOutComplete = false;
        fadeInOutHandler.FadeOut(() =>
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            isFadeOutComplete = true;
        });
        // TODO: 上面的实现不会报错，下面的是实现会报错
        yield return new WaitForSeconds(fadeInOutHandler.fadeDuration + 0.1f);
        // yield return new WaitUntil(() => isFadeOutComplete);
    }
}