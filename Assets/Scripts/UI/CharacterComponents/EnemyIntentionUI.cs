using System.Collections.Generic;
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

        int numEffects = nextTurnEffects.Count;
        if (numEffects == 0) return;

        // 设置左右对称的分布
        for (int i = 0; i < numEffects; i++)
        {
            float iconWidth = effectIconPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            // 计算当前效果的位置
            float offset = (i - (numEffects - 1) / 2f) * (iconWidth + spacing);
            Vector3 iconPosition = centerPosition + new Vector3(offset, 0, 0);

            // 创建图标
            GameObject icon = Instantiate(effectIconPrefab, iconPosition, Quaternion.identity);
            icon.GetComponent<SpriteRenderer>().sprite = nextTurnEffects[i].icon;
            icon.transform.SetParent(transform, false);
        }
    }
    public void ClearUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion
}
