using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEditor;
using UnityEditor.SearchService;

// This scipt manage character dearth, win condition, and gameover condition
public class GameManager : MonoBehaviour
{
    public List<GameObject> enemyList; // Store current live enemies

    [Header("Broadcast Events")]
    public ObjectEventSO battleEndEvent;
    public ObjectEventSO battleStartEvent;
    public ObjectEventSO gameOverEvent;

    public void OnSceneLoadCompleteEvent(object obj)
    {
        GameSceneSO currentScene = obj as GameSceneSO;
        if (currentScene.sceneType == SceneType.Battle)
        {
            // If a battle scene loaded, start battle
            enemyList = GameObject.FindGameObjectsWithTag("enemy").ToList();
            if (enemyList.Count <= 0) Debug.LogWarning("No enemies found in the scene.");
            battleStartEvent.RaiseEvent(this, this);
        }
        //TODO: other type scene action (If have)
    }

    public void OnEnemySpawnEvent(object obj)
    {
        GameObject newEnemy = obj as GameObject;
        enemyList.Add(newEnemy);
    }

    public void OnCharacterDeathEvent(object obj)
    {
        // Check this Dead obj is player or enemy
        if (obj is Player player)
        {
            //Player is dead, gameover
            Debug.Log("Player is dead, gameover");
            gameOverEvent.RaiseEvent(this, this);
        }
        else if (obj is EnemyBase deadEnemy)
        {
            enemyList.Remove(deadEnemy.gameObject);
            Destroy(deadEnemy.gameObject);
            // Check if all enemies are dead
            if (enemyList.Count == 0)
            {
                battleEndEvent.RaiseEvent(this, this);
            }
        }
        else
        {
            Debug.LogWarning("OnCharacterDeathEvent received a null or invalid enemy object.");
        }
    }
}
