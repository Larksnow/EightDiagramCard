using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyArrangement : MonoBehaviour
{
    public List<GameObject> enemyList;
    public float leftX = 0f;
    public float rightX = 8f;

    public void UpdateEnemyPosition()
    {
        CalculateEnemyPosition();
        foreach (var enemy in enemyList)
        {
            enemy.transform.DOMove(new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z), 0.5f);
        }
    }

    private void CalculateEnemyPosition()
    {
        float center = rightX - leftX / 2f;
        float step = (rightX - leftX) / (enemyList.Count + 1);

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].transform.position = new Vector3(leftX + step * (i + 1), enemyList[i].transform.position.y, enemyList[i].transform.position.z);
        }
    }
}
