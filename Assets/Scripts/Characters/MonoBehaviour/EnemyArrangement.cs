using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyArrangement : MonoBehaviour
{
    public GameManager gameManager;
    public float leftX = 0f;
    public float rightX = 8f;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void UpdateEnemyPosition()
    {
        CalculateEnemyPosition();
        foreach (var enemy in gameManager.enemyList)
        {
            enemy.transform.DOMove(new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z), 0.5f);
        }
    }

    private void CalculateEnemyPosition()
    {
        float center = rightX - leftX / 2f;
        float step = (rightX - leftX) / (gameManager.enemyList.Count + 1);

        for (int i = 0; i < gameManager.enemyList.Count; i++)
        {
            gameManager.enemyList[i].transform.position = new Vector3(leftX + step * (i + 1), gameManager.enemyList[i].transform.position.y, gameManager.enemyList[i].transform.position.z);
        }
    }
}
