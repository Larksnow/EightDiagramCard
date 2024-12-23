using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [Header("UI Lists")]
    // 手动添加多种场景ui Lists
    public List<GameObject> testSceneUis;
    public List<GameObject> menuSceneUis;
    public List<GameObject> battleSceneUis;
    
    public SceneLoadManager sceneLoadManager;
    public GameObject backgroundDimmer;
    public GameObject returnButton;
    
    private SceneType firstLoadedSceneType;
    private Dictionary<SceneType, List<GameObject>> sceneUIs = new();
    private SceneType currentSceneType;

    private void Awake()
    {
        if (!sceneLoadManager.enableTest)
        {
            SetUIObjectsActive(testSceneUis,false);
            firstLoadedSceneType = SceneType.Menu;
        }
        else
        {
            firstLoadedSceneType = SceneType.Test;
        }
        
        currentSceneType = firstLoadedSceneType;
        sceneUIs.Add(SceneType.Menu, menuSceneUis);
        sceneUIs.Add(SceneType.Battle, battleSceneUis);
        // 创建并添加其他场景 UI Lists
        
        if (backgroundDimmer != null) backgroundDimmer.SetActive(false);
        if (returnButton != null) returnButton.SetActive(false);
    }

    #region Event Listening

    public void LoadNextSceneUI(object obj)
    {
        var gameSceneSo = (GameSceneSO)obj;
        if (currentSceneType == gameSceneSo.sceneType)
        {
            // // 确保其他场景UI不显示
            // foreach (var (sceneType, uis) in sceneUIs)
            // {
            //     if (sceneType != currentSceneType)
            //         SetUIObjectsActive(uis, false);
            // }
            //
            // SetUIObjectsActive(sceneUIs[currentSceneType], true);
            //
            return;
        }

        SetUIObjectsActive(sceneUIs[currentSceneType], false);
        var nextSceneType = gameSceneSo.sceneType;
        SetUIObjectsActive(sceneUIs[nextSceneType], true);
    }

    #endregion

    private void SetUIObjectsActive(List<GameObject> uis, bool isActive)
    {
        foreach (var obj in uis)
        {
            obj.SetActive(isActive);
        }
    }
}