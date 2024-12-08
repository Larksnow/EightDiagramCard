using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    // 手动添加多种场景ui Lists
    public List<GameObject> menuSceneUis;
    public List<GameObject> battleSceneUis;

    private Dictionary<SceneType, List<GameObject>> sceneUIs = new();
    private SceneType currentSceneType = SceneType.Menu;

    private void Awake()
    {
        sceneUIs.Add(SceneType.Menu, menuSceneUis);
        sceneUIs.Add(SceneType.Battle, battleSceneUis);
        //TODO:创建并添加其他场景 UI Lists
    }

    #region Event Listening

    public void LoadNextSceneUI(object obj)
    {
        var gameSceneSo = obj as GameSceneSO;
        if (currentSceneType == gameSceneSo.sceneType)
        {
            return;
        }

        SetUIObjectsActive(sceneUIs[currentSceneType], false);
        currentSceneType = gameSceneSo.sceneType;
        SetUIObjectsActive(sceneUIs[currentSceneType], true);
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