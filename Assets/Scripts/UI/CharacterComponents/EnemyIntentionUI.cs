using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyIntentionUI : MonoBehaviour
{
    public EnemyBase currentEnemy;
    public GameObject effectIconPrefab;
    public Vector3 centerPosition = new(0, 0, 0);
    public float spacing = 0.2f;
    private List<EnemyEffect> nextTurnEffects = new();

    #region Event Listening

    public void ShowUI()
    {
        nextTurnEffects = currentEnemy.GetNextTurnEffects();

        ClearUI();

        var numEffects = nextTurnEffects.Count;
        if (numEffects == 0) return;

        // 设置左右对称的分布
        for (var i = 0; i < numEffects; i++)
        {
            var iconWidth = effectIconPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            // 计算当前效果的位置
            var offset = (i - (numEffects - 1) / 2f) * (iconWidth + spacing);
            var iconPosition = centerPosition + new Vector3(offset, 0, 0);

            // 创建图标
            var icon = Instantiate(effectIconPrefab, iconPosition, Quaternion.identity);
            icon.GetComponent<SpriteRenderer>().sprite = nextTurnEffects[i].icon;
            icon.GetComponentInChildren<TextMeshPro>().text = nextTurnEffects[i].GetRuntimeValue().ToString();
            icon.transform.SetParent(transform, false);
        }
    }

    private void ClearUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion
}