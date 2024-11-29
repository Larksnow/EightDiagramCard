
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardActivation : MonoBehaviour
{
    public float uiFadeDuration = 1f;

    private TextMeshPro cardActivationText;
    private Vector3 originalTextScale;
    private Queue<CardDataSO> cardDataQueue = new();
    private bool isPlaying;

    private void Awake()
    {
        cardActivationText = GetComponent<TextMeshPro>();
        originalTextScale = cardActivationText.transform.localScale;
    }

    public void ShowCardActivation(object obj)
    {
        CardDataSO cardData = (CardDataSO)obj;

        // 将当前的序列添加到队列中
        cardDataQueue.Enqueue(cardData);

        // 如果当前没有正在播放的动画，就开始播放第一个动画
        if (!isPlaying)
        {
            isPlaying = true;
            Debug.Log("开始播放第一个动画");
            StartCoroutine(PlayNextActivation());
        }
    }

    private IEnumerator PlayNextActivation()
    {
        while (cardDataQueue.Count > 0)
        {
            // 获取并解开队列中的动画和数据
            CardDataSO cardData = cardDataQueue.Dequeue();

            // 更新文本和颜色
            cardActivationText.text = cardData.cardName;
            cardActivationText.color = cardData.color;

            Sequence nextSequence = DOTween.Sequence();
            nextSequence.Append(cardActivationText.transform.DOScale(originalTextScale * 1.2f, uiFadeDuration).SetEase(Ease.OutCubic))
            .Join(cardActivationText.DOFade(0f, uiFadeDuration)).onComplete += () =>
            {
                cardActivationText.transform.localScale = originalTextScale;
                cardActivationText.color = new Color(cardActivationText.color.r, cardActivationText.color.g, cardActivationText.color.b, 1f);
                cardActivationText.text = "";
            };

            // 等待当前动画播放完成
            yield return nextSequence.WaitForCompletion();

            // 动画完成后继续播放下一个动画
            Debug.Log("当前动画播放完成，准备播放下一个动画");
        }

        // 如果队列为空，标记播放结束
        isPlaying = false;
    }
}