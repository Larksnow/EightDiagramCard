using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private bool isPlayTurn;
    private bool isEnemyTurn;

    public bool battleEnd = true;
    

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
        if (battleEnd) return;
        if (isEnemyTurn) {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration) 
            {
                timeCounter = 0f;
                EnemyTurnEnd();
                isPlayTurn = true;
            }
        }
        else if (isPlayTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;
                PlayerTurnBegin();
                isPlayTurn = false;
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
    }

    public void PlayerTurnBegin()
    {
        playerTurnBegin.RaiseEvent(null, this);
    }

    public void EnemyTurnBegin()
    {
        isEnemyTurn = true;
        Debug.Log("Enemy Turn Begin");
        enemyTurnBegin.RaiseEvent(null, this);
    }

    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        Debug.Log("Enemy Turn End");
        enemyTurnEnd.RaiseEvent(null, this);
    }
}
