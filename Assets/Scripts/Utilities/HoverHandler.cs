using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 处理鼠标悬停事件
/// </summary>
public class HoverHandler : MonoBehaviour
{
    [Header("Enlargeable")]
    public bool enlargable;
    public float scaleMultiplier;
    public List<Transform> enlargedTransforms;

    [Header("Hover Panel")]
    public bool hasHoverPanel;
    public GameObject hoverPanel;

    private List<Vector3> originalScales = new();

    private void Awake()
    {
        foreach (Transform t in enlargedTransforms)
        {
            originalScales.Add(t.localScale);
        }
    }

    private void OnMouseEnter()
    {
        if (enlargable)
        {
            for (int i = 0; i < enlargedTransforms.Count; i++)
            {
                enlargedTransforms[i].localScale = originalScales[i] * scaleMultiplier;
            }
        }
        if (hasHoverPanel)
        {
            hoverPanel.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (enlargable)
        {
            for (int i = 0; i < enlargedTransforms.Count; i++)
            {
                enlargedTransforms[i].localScale = originalScales[i];
            }
        }
        if (hasHoverPanel)
        {
            hoverPanel.SetActive(false);
        }
    }
    // TODO:鼠标点击放大动画
}