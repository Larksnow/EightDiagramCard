using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectDiagramPanel : MonoBehaviour
{
    private PauseManager pauseManager;
    public CopyDiagramEffect copyDiagramEffect;
    public DiagramDataSO kunData, zhenData, xunData, kanData, liData, genData, duiData;
    public float fadeDuration = 0.5f;

    private Dictionary<string, DiagramDataSO> diagramDataMapping;
    private SpriteRenderer panelBackground;
    private TextMeshPro[] diagramTexts;
    private SpriteRenderer[] diagramImages;

    private void Awake()
    {
        pauseManager = PauseManager.Instance;
        // 初始化字典映射
        diagramDataMapping = new Dictionary<string, DiagramDataSO>
        {
            { "Kun", kunData },
            { "Zhen", zhenData },
            { "Xun", xunData },
            { "Kan", kanData },
            { "Li", liData },
            { "Gen", genData },
            { "Dui", duiData }
        };
        panelBackground = GetComponent<SpriteRenderer>();
        diagramTexts = GetComponentsInChildren<TextMeshPro>();
        diagramImages = GetComponentsInChildren<SpriteRenderer>();
    }
    private void OnEnable()
    {
        FadeIn();
        pauseManager.PauseGame();
    }
    private void OnDisable()
    {
        pauseManager.UnpauseGame();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.transform.IsChildOf(transform))
            {
                // 检查点击的是否是 SelectDiagramPanel 的子物体
                string diagramName = hit.collider.gameObject.name;
                Debug.Log($"Clicked on: {diagramName}");

                if (diagramDataMapping.TryGetValue(diagramName, out var diagramData))
                {
                    copyDiagramEffect.diagramDataToCopy = diagramData; // 处理子物体点击逻辑
                    HighlightButton(hit.collider.gameObject);
                }
            }
        }
    }

    // 玩家选择按钮后关闭面板
    private void HighlightButton(GameObject diagram)
    {
        var text = diagram.GetComponentInChildren<TextMeshPro>();
        var sr = diagram.GetComponentInChildren<SpriteRenderer>();
        Vector3 textOriginalScale = text.transform.localScale;
        Vector3 srOriginalScale = sr.transform.localScale;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(text.transform.DOScale(textOriginalScale * 1.2f, 0.5f)).Join(sr.transform.DOScale(srOriginalScale * 1.2f, 0.5f)).OnComplete(() =>
        {
            text.transform.localScale = textOriginalScale;
            sr.transform.localScale = srOriginalScale;
            StartCoroutine(FadeOutCoroutine());
        });
    }

    private void FadeIn()
    {
        SetAllElementsAlpha(0); // 初始化透明度
        Sequence sequence = DOTween.Sequence();
        sequence.Append(panelBackground.DOFade(1, fadeDuration));
        foreach (var text in diagramTexts)
        {
            sequence.Join(text.DOFade(1, fadeDuration));
        }
        foreach (var image in diagramImages)
        {
            sequence.Join(image.DOFade(1, fadeDuration));
        }
    }
    private IEnumerator FadeOutCoroutine()
    {
        FadeOut();
        yield return new WaitForSeconds(fadeDuration);
        gameObject.SetActive(false); // 隐藏面板
    }
    private void FadeOut()
    {
        SetAllElementsAlpha(1); // 还原透明度
        Sequence sequence = DOTween.Sequence();
        sequence.Append(panelBackground.DOFade(0, fadeDuration));
        foreach (var text in diagramTexts)
        {
            sequence.Join(text.DOFade(0, fadeDuration));
        }
        foreach (var image in diagramImages)
        {
            sequence.Join(image.DOFade(0, fadeDuration));
        }
    }

    private void SetAllElementsAlpha(float alpha)
    {
        panelBackground.color = new Color(panelBackground.color.r, panelBackground.color.g, panelBackground.color.b, alpha);
        foreach (var text in diagramTexts)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
        foreach (var image in diagramImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
    }
}