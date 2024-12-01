using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private bool isPlayTurn;
    private bool isEnemyTurn;

    public bool battleEnd = true;
    public int enemyFinishCount;
    

    // These time counter are used to wait a short time in trun change
    private float timeCounter;

    public float enemyTurnDuration;
    public float playerTurnDuration;

    [Header("Broadcast Events")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;
    public GameObject[] enemyList;
    public ObjectEventSO battleEndEvent;
    

    private void Update()
    {
        if (battleEnd) return;
        if (AreAllEnemiesDead())
        {
            battleEndEvent.RaiseEvent(null, this);
            battleEnd = true;
        }
        if (isEnemyTurn) {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration) 
            {
                timeCounter = 0f;
                EnemyTurnBegin();
                isEnemyTurn = false;
            }
        }

    }
    private bool AreAllEnemiesDead()
    {
        foreach (var enemy in enemyList)
        {
            if (enemy != null)
            {
                // Assuming each enemy has a script with a 'isDead' property
                var enemyScript = enemy.GetComponent<EnemyBase>();
                if (enemyScript != null && !enemyScript.isDead)
                {
                    return false; // If any enemy is not dead, return false
                }
            }
        }
        return true; // All enemies are dead
    }

    [ContextMenu("Battle Start")]
    public void BattleStart()
    {
        isPlayTurn = true;
        isEnemyTurn = false;
        battleEnd = false;
        timeCounter = 0;
        PlayerTurnBegin();
        enemyList = GameObject.FindGameObjectsWithTag("enemy");
    }

    public void PlayerTurnBegin()
    {
        Debug.Log("Player Turn Begin");
        playerTurnBegin.RaiseEvent(null, this);
    }
    
    public void SetEnemyTurnTrue()
    {
        isEnemyTurn = true;
    }
    public void EnemyTurnBegin()
    {
        Debug.Log("Enemy Turn Begin");
        enemyTurnBegin.RaiseEvent(null, this);
    }

    [ContextMenu("EnemyTurnEnd")]
    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        Debug.Log("Enemy Turn End");
        enemyTurnEnd.RaiseEvent(null, this);
    }

    public void CountAllEnemyFinish()
    {
        enemyFinishCount++;
        if (enemyFinishCount == enemyList.Count())
        {
            Debug.Log("All enemy finshed action");
            PlayerTurnBegin();
            enemyFinishCount = 0;
        }
    }
}
