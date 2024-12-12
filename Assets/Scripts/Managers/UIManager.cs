using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    // 手动添加多种场景ui Lists
    public List<GameObject> menuSceneUis;
    public List<GameObject> battleSceneUis;
    public SceneType firstLoadedSceneType = SceneType.Menu;

    private Dictionary<SceneType, List<GameObject>> sceneUIs = new();
    private SceneType currentSceneType;

    private void Awake()
    {
        currentSceneType = firstLoadedSceneType;
        sceneUIs.Add(SceneType.Menu, menuSceneUis);
        sceneUIs.Add(SceneType.Battle, battleSceneUis);
        // 创建并添加其他场景 UI Lists
    }

    #region Event Listening

    public void LoadNextSceneUI(object obj)
    {
        var gameSceneSo = (GameSceneSO)obj;
        // 测试场景默认所有UI都显示
        if (gameSceneSo.sceneType == SceneType.Test) return;
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