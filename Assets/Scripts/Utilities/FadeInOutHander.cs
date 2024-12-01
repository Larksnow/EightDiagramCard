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

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        spriteRenderers.AddRange(GetComponents<SpriteRenderer>());
        textMeshPros.AddRange(GetComponentsInChildren<TextMeshPro>());
        textMeshPros.AddRange(GetComponents<TextMeshPro>());
        images.AddRange(GetComponentsInChildren<Image>());
        images.AddRange(GetComponents<Image>());
    }

    public void FadeIn()
    {
        Init();
        SetAllElementsAlpha(0); // 初始化透明度
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
    }

    public void FadeOut()
    {
        SetAllElementsAlpha(1); // 还原透明度
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
    }
}