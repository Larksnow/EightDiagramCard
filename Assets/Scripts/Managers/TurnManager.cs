using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
// This scipt manage trun during the battle
public class TurnManager : MonoBehaviour
{
    public int enemyFinishCount;
    public GameManager gameManager;
    private float turnSwitchTime = 1.5f; 
    // These time counter are used to wait a short time in trun change

    public float enemyTurnDuration;
    public float playerTurnDuration;

    [Header("Broadcast Events")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;
   

    [ContextMenu("Battle Start")]
    public void BattleStart()
    {
        enemyFinishCount = 0;
        StartCoroutine(PlayerTurnBeginCoroutine());
    }

    IEnumerator PlayerTurnBeginCoroutine()
    {
        yield return new WaitForSeconds(turnSwitchTime);
        Debug.Log("Player Turn Begin");
        playerTurnBegin.RaiseEvent(null, this);
    }

    public void EnemyTurnBegin()
    {
        StartCoroutine(EnemyTurnBeginCoroutine());
    }
    IEnumerator EnemyTurnBeginCoroutine()
    {
        yield return new WaitForSeconds(turnSwitchTime);
        Debug.Log("Enemy Turn Begin");
        enemyTurnBegin.RaiseEvent(null, this);
    }

    public void CheckAllEnemiesFinished()
    {
        enemyFinishCount++;
        if (enemyFinishCount == gameManager.enemyList.Count())
        {
            Debug.Log("All enemy finshed action");
            enemyFinishCount = 0;
            StartCoroutine(PlayerTurnBeginCoroutine());
        }
    }
}
