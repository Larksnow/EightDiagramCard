using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    private bool isPaused;

    // 对于诸如面板卦象，牌堆按钮等可交互UI，打开后暂停除自身外其他所有可交互UI
    private List<GameObject> excludeFromPause = new();   

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PauseGame(List<GameObject> excludeList)
    {
        excludeFromPause.AddRange(excludeList);
        isPaused = true;
    }
    public void ResumeGame()
    {
        excludeFromPause.Clear();
        isPaused = false;
    }
    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsInExcludeList(GameObject obj)
    {
        return excludeFromPause.Contains(obj);
    }
}