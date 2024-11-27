using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 处理鼠标悬停事件
/// </summary>
public class HoverHandler : MonoBehaviour
{
    public float scaleMultiplier;
    public List<Transform> targetTransforms;

    private List<Vector3> originalScales = new();

    private void Awake()
    {
        foreach (Transform t in targetTransforms)
        {
            originalScales.Add(t.localScale);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("OnPointerEnter");
        for (int i = 0; i < targetTransforms.Count; i++)
        {
            targetTransforms[i].localScale = originalScales[i] * scaleMultiplier;
        }
    }

    private void OnMouseExit()
    {
        Debug.Log("OnPointerExit");
        for (int i = 0; i < targetTransforms.Count; i++)
        {
            targetTransforms[i].localScale = originalScales[i];
        }
    }
}