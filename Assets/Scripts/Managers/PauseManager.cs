using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    private bool isPaused;
    public ButtonsManager buttonsManager;

    // private List<GameObject> excludeFromPause = new();   

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
        buttonsManager = ButtonsManager.Instance;
    }

    /// <summary>
    /// 暂停游戏，默认将场中所有按钮设为不可交互
    /// </summary>
    public void PauseGame(List<Button> pauseCullBtns = null)
    {
        // excludeFromPause.Clear();
        // excludeFromPause.AddRange(excludeList);
        isPaused = true;
        buttonsManager.SetAllButtonsInteractable(false);
        if (pauseCullBtns != null)
            buttonsManager.SetButtonsInteractable(pauseCullBtns, true);
    }
    public void ResumeGame()
    {
        // excludeFromPause.Clear();
        isPaused = false;
        buttonsManager.SetAllButtonsInteractable(true);
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    // public bool IsInExcludeList(GameObject obj)
    // {
    //     return excludeFromPause.Contains(obj);
    // }
}