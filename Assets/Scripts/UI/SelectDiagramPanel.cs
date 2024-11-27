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

    private Dictionary<string, DiagramDataSO> diagramDataMapping;

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
    }
    private void OnEnable() {
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
                string diagramName = hit.collider.transform.parent.name;
                Debug.Log($"Clicked on: {diagramName}");

                if (diagramDataMapping.TryGetValue(diagramName, out var diagramData))
                {
                    copyDiagramEffect.diagramDataToCopy = diagramData; // 处理子物体点击逻辑
                    HighlightButton(hit.collider.transform.parent.gameObject);
                }
            }
        }
    }

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
            gameObject.SetActive(false); // 隐藏面板
        });
    }
}