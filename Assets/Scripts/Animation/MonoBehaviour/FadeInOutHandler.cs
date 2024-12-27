using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutHandler : MonoBehaviour
{
    public float fadeDuration = 0.8f;

    private CanvasGroup canvasGroup = null;
    private List<Graphic> graphics = new();
    private List<SpriteRenderer> spriteRenderers = new();

    private void Awake()
    {
        UpdateUIElements();
    }

    #region Event Listening

    // 更新所有需要透明度变化的UI元素
    // 在需要动态增删子物体的GameObject中调用
    public void UpdateUIElements()
    {
        if (!TryGetComponent(out canvasGroup))
        {
            graphics.Clear();
            spriteRenderers.Clear();

            graphics.AddRange(GetComponentsInChildren<Graphic>());
            graphics.AddRange(GetComponents<Graphic>());
            spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
            spriteRenderers.AddRange(GetComponents<SpriteRenderer>());
        }
    }

    #endregion

    public void FadeIn(Action onComplete = null, float endAlpha = 1)
    {
        SetAllElementsAlpha(0); // 初始化透明度为0
        StartCoroutine(FadeInCoroutine(onComplete, endAlpha));
    }

    public void FadeOut(Action onComplete = null)
    {
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    private IEnumerator FadeInCoroutine(Action onComplete, float endAlpha)
    {
        Sequence sequence = DOTween.Sequence().SetEase(Ease.InOutExpo);

        if (canvasGroup != null)
        {
            sequence.Join(canvasGroup.DOFade(endAlpha, fadeDuration));
        }
        else
        {
            foreach (var graphic in graphics)
            {
                sequence.Join(graphic.DOFade(endAlpha, fadeDuration));
            }

            foreach (var render in spriteRenderers)
            {
                sequence.Join(render.DOFade(endAlpha, fadeDuration));
            }
        }

        yield return sequence.WaitForCompletion();
        onComplete?.Invoke();
    }

    private IEnumerator FadeOutCoroutine(Action onComplete)
    {
        Sequence sequence = DOTween.Sequence().SetEase(Ease.InOutExpo);

        if (canvasGroup != null)
        {
            sequence.Join(canvasGroup.DOFade(0, fadeDuration));
        }
        else
        {
            foreach (var graphic in graphics)
            {
                sequence.Join(graphic.DOFade(0, fadeDuration));
            }

            foreach (var render in spriteRenderers)
            {
                sequence.Join(render.DOFade(0, fadeDuration));
            }
        }

        yield return sequence.WaitForCompletion();
        onComplete?.Invoke();
    }

    /// <summary>
    /// 控制具体某个物体的淡入淡出
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="onComplete"></param>
    /// <param name="endAlpha"></param>
    /// <param name="duration"></param>
    public static void FadeObjectIn(GameObject obj, Action onComplete = null, float endAlpha = 1, float duration = 0.8f)
    {
        if (!obj.TryGetComponent<FadeInOutHandler>(out var handler))
        {
            handler = obj.AddComponent<FadeInOutHandler>();
        }

        handler.fadeDuration = duration;
        handler.FadeIn(onComplete, endAlpha);
    }

    public static void FadeObjectOut(GameObject obj, Action onComplete = null, float duration = 0.8f)
    {
        if (!obj.TryGetComponent<FadeInOutHandler>(out var handler))
        {
            handler = obj.AddComponent<FadeInOutHandler>();
        }

        handler.fadeDuration = duration;
        handler.FadeOut(onComplete);
    }

    // 设置所有UI元素的透明度
    private void SetAllElementsAlpha(float alpha)
    {
        foreach (var graphic in graphics)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
        }

        foreach (var render in spriteRenderers)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b,
                alpha);
        }
    }
}