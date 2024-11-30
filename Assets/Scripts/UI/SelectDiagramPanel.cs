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
    public FadeInOutHander fadeInOutHander;

    private Dictionary<string, DiagramDataSO> diagramDataMapping;
    private SpriteRenderer panelBackground;
    private TextMeshPro[] diagramTexts;
    private TextMeshPro[] diagramEnhancedTexts;
    private SpriteRenderer[] diagramImages;

    private void Awake()
    {
        pauseManager = PauseManager.Instance;
        // 初始化字典映射
        diagramDataMapping = new Dictionary<string, DiagramDataSO>
        {
            { kunData.diagramName, kunData },
            { zhenData.diagramName, zhenData },
            { xunData.diagramName, xunData },
            { kanData.diagramName, kanData },
            { liData.diagramName, liData },
            { genData.diagramName, genData },
            { duiData.diagramName, duiData }
        };
        panelBackground = GetComponent<SpriteRenderer>();
        diagramTexts = transform.Find("Text").GetComponents<TextMeshPro>();
        diagramEnhancedTexts = transform.Find("EnhancedText").GetComponents<TextMeshPro>();
        diagramImages = GetComponentsInChildren<SpriteRenderer>();
        InitializeDiagramVisuals();
    }
    private void OnEnable()
    {
        SetEnhancedTexts();
        fadeInOutHander.FadeIn();
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

    private IEnumerator FadeOutCoroutine()
    {
        fadeInOutHander.FadeOut();
        yield return new WaitForSeconds(fadeInOutHander.fadeDuration);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Load data (color, text, sprite) from DiagramDataSO
    /// </summary>
    private void InitializeDiagramVisuals()
    {
        foreach (var pair in diagramDataMapping)
        {
            string diagramName = pair.Key;
            DiagramDataSO diagramData = pair.Value;

            // Find the corresponding child GameObject by name
            Transform diagramTransform = transform.Find(diagramName);
            if (diagramTransform == null) continue;

            // Set the diagram's color, name, and pattern
            var text = diagramTransform.GetComponentInChildren<TextMeshPro>();
            var image = diagramTransform.GetComponentInChildren<SpriteRenderer>();

            if (text != null)
            {
                text.text = diagramData.diagramName; // Update the diagram name
                text.color = diagramData.diagramColor;
                text.outlineWidth = 0.05f;
                text.outlineColor = Color.black;
            }
            if (image != null)
            {
                image.sprite = diagramData.patternSprite; // Update the pattern
            }
        }
    }

    private void SetEnhancedTexts()
    {
        foreach (var text in diagramEnhancedTexts)
        {
            text.enabled = false;
        }

        foreach (var diagramData in copyDiagramEffect.enhancedCopyDiagram)
        {
            transform.Find(diagramData.diagramName).Find("EnhancedText").GetComponent<TextMeshPro>().enabled = true;
        }
    }
}