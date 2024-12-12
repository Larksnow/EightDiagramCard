using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectDiagramPanel : FadablePanel, IButtonClickHandler
{
    public CopyDiagramEffect copyDiagramEffect;
    public DiagramDataSO kunData, zhenData, xunData, kanData, liData, genData, duiData;

    private Dictionary<string, DiagramDataSO> diagramDataMapping;
    private List<TextMeshPro> diagramEnhancedTexts = new();
    private List<GameObject> excludeFromPauseList = new();

    protected override void Awake()
    {
        base.Awake();
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
        // 寻找子物体文本、图像组件
        foreach (Transform child in transform)
        {
            if (child != null)
            {
                // 将可点击组件加入剔除暂停列表，不受暂停影响
                excludeFromPauseList.Add(child.gameObject);

                diagramEnhancedTexts.Add(child.Find("EnhancedText").GetComponent<TextMeshPro>());
            }
        }

        InitializeDiagramVisuals();
    }

    protected override void OnEnable()
    {
        SetEnhancedTexts();
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnClickSelected(GameObject selected)
    {
        base.OnClickSelected(selected);
        string diagramName = selected.name;
        if (diagramDataMapping.TryGetValue(diagramName, out var diagramData))
        {
            copyDiagramEffect.diagramDataToCopy = diagramData;
        }
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