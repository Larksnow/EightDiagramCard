using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 处理挂载该脚本的GameObject的SpriteRenderer和TextMeshPro的FadeIn和FadeOut效果
public class FadeInOutHander : MonoBehaviour
{
    public float fadeDuration = 0.8f;
    private List<SpriteRenderer> spriteRenderers = new();
    private List<TextMeshPro> textMeshPros = new();
    private List<Image> images = new();
    private List<TextMeshProUGUI> textMeshProUGUIs = new();
    private List<CanvasGroup> canvasGroups = new();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        spriteRenderers.Clear();
        textMeshPros.Clear();
        images.Clear();
        textMeshProUGUIs.Clear();
        canvasGroups.Clear();

        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        spriteRenderers.AddRange(GetComponents<SpriteRenderer>());
        textMeshPros.AddRange(GetComponentsInChildren<TextMeshPro>());
        textMeshPros.AddRange(GetComponents<TextMeshPro>());
        images.AddRange(GetComponentsInChildren<Image>());
        images.AddRange(GetComponents<Image>());
        textMeshProUGUIs.AddRange(GetComponentsInChildren<TextMeshProUGUI>());
        textMeshProUGUIs.AddRange(GetComponents<TextMeshProUGUI>());
        canvasGroups.AddRange(GetComponentsInChildren<CanvasGroup>());
        canvasGroups.AddRange(GetComponents<CanvasGroup>());
    }

    public void FadeIn(Action onComplete = null)
    {
        Init();
        SetAllElementsAlpha(0); // 初始化透明度
        StartCoroutine(FadeInCoroutine(onComplete));
    }
    private IEnumerator FadeInCoroutine(Action onComplete)
    {
        Sequence sequence = DOTween.Sequence().SetEase(Ease.InOutExpo);
        foreach (var renderer in spriteRenderers)
        {
            sequence.Join(renderer.DOFade(1, fadeDuration));
        }
        foreach (var text in textMeshPros)
        {
            sequence.Join(text.DOFade(1, fadeDuration));
        }
        foreach (var image in images)
        {
            sequence.Join(image.DOFade(1, fadeDuration));
        }
        foreach (var text in textMeshProUGUIs)
        {
            sequence.Join(text.DOFade(1, fadeDuration));
        }
        foreach (var canvasGroup in canvasGroups)
        {
            sequence.Join(canvasGroup.DOFade(1, fadeDuration));
        }
        yield return new WaitForSeconds(fadeDuration);
        onComplete?.Invoke();
    }

    public void FadeOut(Action onComplete = null)
    {
        SetAllElementsAlpha(1); // 还原透明度
        StartCoroutine(FadeOutCoroutine(onComplete));
    }
    private IEnumerator FadeOutCoroutine(Action onComplete)
    {
        Sequence sequence = DOTween.Sequence().SetEase(Ease.InOutExpo);
        foreach (var text in textMeshPros)
        {
            sequence.Join(text.DOFade(0, fadeDuration));
        }
        foreach (var renderer in spriteRenderers)
        {
            sequence.Join(renderer.DOFade(0, fadeDuration));
        }
        foreach (var image in images)
        {
            sequence.Join(image.DOFade(0, fadeDuration));
        }
        foreach (var text in textMeshProUGUIs)
        {
            sequence.Join(text.DOFade(0, fadeDuration));
        }
        foreach (var canvasGroup in canvasGroups)
        {
            sequence.Join(canvasGroup.DOFade(0, fadeDuration));
        }
        yield return new WaitForSeconds(fadeDuration);
        onComplete?.Invoke();
    }

    private void SetAllElementsAlpha(float alpha)
    {
        foreach (var renderer in spriteRenderers)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
        }
        foreach (var text in textMeshPros)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
        foreach (var image in images)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
        foreach (var text in textMeshProUGUIs)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
        foreach (var canvasGroup in canvasGroups)
        {
            canvasGroup.alpha = alpha;
        }
    }
}