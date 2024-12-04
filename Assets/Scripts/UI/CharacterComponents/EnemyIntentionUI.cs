using System.Collections.Generic;
using UnityEngine;

public class EnemyIntentionUI : MonoBehaviour
{
    public EnemyBase currentEnemy;
    public GameObject effectIconPrefab;
    public Vector3 centerPosition = new(-0.8f, 2.5f, 0);
    public Vector3 iconScale = new(0.3f, 0.3f, 0);
    public float spacing = 0.5f;
    private List<EnemyEffect> nextTurnEffects = new();

    #region Event Listening
    public void ShowUI()
    {
        nextTurnEffects = currentEnemy.GetNextTurnEffects();

        HideUI();

        int numEffects = nextTurnEffects.Count;
        if (numEffects == 0) return;

        // 设置左右对称的分布
        float totalWidth = numEffects * iconScale.x;

        // 根据效果数量进行分布
        for (int i = 0; i < numEffects; i++)
        {
            // 计算当前效果的位置
            float offset = (i - (numEffects - 1) / 2f) * (iconScale.x + spacing);
            Vector3 iconPosition = centerPosition + new Vector3(offset, 0, 0);

            // 创建图标
            GameObject icon = Instantiate(effectIconPrefab, iconPosition, Quaternion.identity);
            icon.GetComponent<SpriteRenderer>().sprite = nextTurnEffects[i].icon;
            icon.transform.SetParent(transform, false);
            icon.transform.localScale = iconScale;
        }
    }
    public void HideUI()
    {
        foreach (GameObject icon in transform)
        {
            Destroy(icon);
        }
    }
    #endregion
}
