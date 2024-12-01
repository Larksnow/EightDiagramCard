using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [Header("Scenes")]
    public GameSceneSO menuScene;
    public GameSceneSO battleScene1;
    public GameSceneSO battleScene2;
    public GameSceneSO battleScene3;
    public GameSceneSO battleScene4;
    List<GameSceneSO> battleScenes = new();

    public GameObject fadeCanvas;
    public List<GameObject> persistentObjectsInBattle;
    private int currentLevel = 0;

    [SerializeField] private GameSceneSO currentLoadedScene;
    [SerializeField] private GameSceneSO sceneToLoad;

    private void Start()
    {
        battleScenes.Add(battleScene1);
        battleScenes.Add(battleScene2);
        battleScenes.Add(battleScene3); 
        battleScenes.Add(battleScene4);
        // 刚开始加载菜单场景
        OnLoadRequest(menuScene);
    }

    #region Event Listening
    public void NextLevel()
    {
        OnLoadRequest(battleScenes[++currentLevel]);
    }

    public void OnLoadRequest(object obj)
    {
        GameSceneSO gameSceneToGo = (GameSceneSO)obj;
        switch (gameSceneToGo.sceneType)
        {
            case SceneType.Menu:
                SetBattlePersistentObjectsActive(false);
                sceneToLoad = menuScene;
                break;
            case SceneType.Battle:
                // TODO: 多个战斗场景
                sceneToLoad = gameSceneToGo;
                break;
        }

        if (currentLoadedScene == null)
        {
            LoadNewScene();
        }
        else
        {
            StartCoroutine(UnloadPreviousScene());
        }
    }
    #endregion

    private IEnumerator UnloadPreviousScene()
    {
        fadeCanvas.SetActive(true);
        FadeInOutHander fadeInOutHander = fadeCanvas.GetComponent<FadeInOutHander>();
        Debug.Log("Start to unload scene: " + currentLoadedScene.sceneName);
        fadeInOutHander.FadeIn();
        yield return new WaitForSeconds(fadeInOutHander.fadeDuration);
        yield return currentLoadedScene.sceneReference.UnLoadScene();
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        if (sceneToLoad.sceneType != SceneType.Menu)
        {
            SetBattlePersistentObjectsActive(true);
        }
        Debug.Log("Start to load scene: " + sceneToLoad.sceneName);
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        Debug.Log("Scene Loaded: " + handle.Result.Scene);
        currentLoadedScene = sceneToLoad;
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        FadeInOutHander fadeInOutHander = fadeCanvas.GetComponent<FadeInOutHander>();
        fadeInOutHander.FadeOut();
        yield return new WaitForSeconds(fadeInOutHander.fadeDuration);
        fadeCanvas.SetActive(false);
    }

    private void SetBattlePersistentObjectsActive(bool isActive)
    {
        foreach (GameObject obj in persistentObjectsInBattle)
        {
            obj.SetActive(isActive);
        }
    }
}
