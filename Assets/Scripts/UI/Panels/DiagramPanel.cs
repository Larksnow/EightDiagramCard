using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using TMPro;
using UnityEngine;
// 卦象计数器UI显示
public class DiagramPanel : MonoBehaviour
{
    public float verticalSpace;
    public GameObject yao;
    public Sprite yinSprite;
    public Sprite yangSprite;
    public float animationDuration;

    public DiagramChecker diagramChecker;
    public TextMeshPro triggerDiagramText;
    private int maxCount = 6;

    [SerializeField] private List<GameObject> yaos = new();

    #region Called by GamePlayPannel
    public void AddOneYao(int cardType)
    {
        GameObject newYao = Instantiate(yao, transform);
        newYao.GetComponent<SpriteRenderer>().sprite = cardType == 0 ? yinSprite : yangSprite;
        
        // 更新Yao的Hover面板
        var hoverPanel = newYao.transform.GetChild(0).gameObject;
        hoverPanel.transform.GetChild(0).GetComponent<TextMeshPro>().text = diagramChecker.yaoList[0].cardName;
        hoverPanel.transform.GetChild(1).GetComponent<TextMeshPro>().text = diagramChecker.yaoList[0].cardDescription;
        
        yaos.Add(newYao);
        if (yaos.Count > maxCount)
        {
            Destroy(yaos[0]);
            yaos.RemoveAt(0);
        }

        UpdateYaoPositions();
    }

    public void TriggerDiagram(DiagramDataSO diagramData, float fadeDuration)
    {
        DOTween.Kill(triggerDiagramText, true);
        HighlightTop3();

        Vector3 originalTextScale = triggerDiagramText.transform.localScale;
        Sequence textAnimationSequence = DOTween.Sequence();
        triggerDiagramText.text = diagramData.diagramName;
        triggerDiagramText.color = diagramData.diagramColor;
        textAnimationSequence.Append(triggerDiagramText.transform.DOScale(originalTextScale * 1.5f, animationDuration).SetEase(Ease.OutCubic)).SetId(triggerDiagramText).Join(triggerDiagramText.DOFade(0f, fadeDuration)).SetId(triggerDiagramText).onComplete = () =>
        {
            triggerDiagramText.text = "";
            triggerDiagramText.transform.localScale = originalTextScale;
        };
    }
    public void ResetDiagramPannel()
    {
        foreach (GameObject yao in yaos)
        {
            SpriteRenderer sprite = yao.GetComponent<SpriteRenderer>();
            sprite.DOFade(0f, animationDuration).OnComplete(() => Destroy(yao));
        }
        yaos.Clear();
    }
    #endregion

    // 触发一卦时高亮所在的三个爻
    private void HighlightTop3()
    {
        if (yaos.Count < 3) return;
        for (int i = yaos.Count - 3; i < yaos.Count; i++)
        {
            GameObject yao = yaos[i];
            Sequence sequence = DOTween.Sequence();
            sequence.Append(yao.transform.DOScale(1.2f, animationDuration).SetEase(Ease.OutCubic));
            sequence.Append(yao.transform.DOScale(1f, animationDuration).SetEase(Ease.InCubic));
        }
    }

    private void UpdateYaoPositions()
    {
        for (int i = 0; i < yaos.Count; i++)
        {
            GameObject yao = yaos[i];
            Vector3 newPos = transform.position + new Vector3(0, i * verticalSpace, 0);
            yao.transform.DOMove(newPos, animationDuration).SetEase(Ease.OutExpo);
        }
    }

}