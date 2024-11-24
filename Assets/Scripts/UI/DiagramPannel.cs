using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DiagramPannel : MonoBehaviour
{
    public float verticalSpace;
    public GameObject yao;
    public Sprite yinSprite;
    public Sprite yangSprite;
    public Color highlightColor = Color.red;
    public float animationDuration;

    private int maxCount = 6;

    private List<GameObject> yaos = new();

    public void AddOneYao(int cardType)
    {
        GameObject newYao = Instantiate(yao, transform);
        newYao.GetComponent<SpriteRenderer>().sprite = cardType == 0 ? yinSprite : yangSprite;
        yaos.Insert(0, newYao);
        if (yaos.Count > maxCount)
        {
            Destroy(yaos[^1]);
            yaos.RemoveAt(yaos.Count - 1);
        }

        UpdateYaoPositions();
    }

    // 触发一卦时高亮所在的三个爻
    public void HighlightTop3()
    {
        if (yaos.Count < 3) return;
        for (int i = yaos.Count - 3; i < yaos.Count; i++) {
            GameObject yao = yaos[i];
            Sequence sequence = DOTween.Sequence();
            sequence.Append(yao.transform.DOScale(1.2f, animationDuration).SetEase(Ease.OutCubic));
            sequence.Append(yao.transform.DOScale(1f, animationDuration).SetEase(Ease.InCubic));
        }
    }

    private void UpdateYaoPositions()
    {
        // 第一个爻始终在最下方, 不需要更新
        if (yaos.Count <= 1) return;
        for (int i = 1; i < yaos.Count; i++)
        {
            GameObject yao = yaos[i];
            Vector3 newPos = yao.transform.position + new Vector3(0, verticalSpace, 0);
            yao.transform.DOMove(newPos, animationDuration).SetEase(Ease.OutExpo);
        }
    }
}