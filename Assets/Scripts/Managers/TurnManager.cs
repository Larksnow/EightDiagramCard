using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// This scipt manage trun during the battle
public class TurnManager : MonoBehaviour
{
    private bool isPlayTurn;
    private bool isEnemyTurn;

    public bool battleEnd = true;
    public int enemyFinishCount;
    public GameManager gameManager;

    // These time counter are used to wait a short time in trun change
    private float timeCounter;

    public float enemyTurnDuration;
    public float playerTurnDuration;

    [Header("Broadcast Events")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;

    private void Update()
    {
        if (battleEnd) return; // if battle is end, don't do anything
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
   

    [ContextMenu("Battle Start")]
    public void BattleStart()
    {
        isPlayTurn = true;
        isEnemyTurn = false;
        battleEnd = false;
        timeCounter = 0;
        enemyFinishCount = 0;
        PlayerTurnBegin();
    }

    public void BattleEnd()
    {
        isPlayTurn = false;
        isEnemyTurn = false;
        battleEnd = true;
        enemyFinishCount = 0;
        timeCounter = 0;
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

    public void CheckAllEnemiesFinished()
    {
        enemyFinishCount++;
        if (enemyFinishCount == gameManager.enemyList.Count())
        {
            Debug.Log("All enemy finshed action");
            PlayerTurnBegin();
            enemyFinishCount = 0;
        }
    }
}
