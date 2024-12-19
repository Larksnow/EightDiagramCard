using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoadManager : MonoBehaviour
{
    public bool enableTest;
    public GameObject fadeImage;

    [Header("Scenes")] public GameSceneSO testSceneSo;
    public GameSceneSO menuSceneSo;
    public List<GameSceneSO> battleSceneSos; // 在inspector中添加battleScenes

    [Header("Broadcast Events")] public ObjectEventSO sceneUnloadCompleteEvent;
    public ObjectEventSO sceneLoadCompleteEvent;

    private int currentLevel = 0;
    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Scene testScene;

    private void Start()
    {
        if (enableTest)
        {
            currentLoadedScene = testSceneSo;
            sceneLoadCompleteEvent.RaiseEvent(currentLoadedScene, this);
        }
        else
        {
            // 卸载默认打开的Test Scene
            testScene = SceneManager.GetSceneByName("Test Scene");
            if (testScene.IsValid() && testScene.isLoaded)
            {
                // 首先将场景所有物体禁用
                foreach (GameObject obj in testScene.GetRootGameObjects())
                {
                    obj.SetActive(false);
                }

                SceneManager.UnloadSceneAsync(testScene);
            }

            // 开始加载菜单场景
            OnLoadRequest(menuSceneSo);
        }
    }

    [ContextMenu("LoadMenu")]
    public void LoadMenu()
    {
        OnLoadRequest(menuSceneSo);
    }

    #region Event Listening

    public void NextLevel()
    {
        if (battleSceneSos.Count < currentLevel + 1)
        {
            Debug.LogError("No more levels to load");
            return;
        }

        OnLoadRequest(battleSceneSos[++currentLevel]);
    }

    public void OnLoadRequest(object obj)
    {
        GameSceneSO gameSceneToGo = (GameSceneSO)obj;
        switch (gameSceneToGo.sceneType)
        {
            case SceneType.Menu:
                sceneToLoad = menuSceneSo;
                break;
            case SceneType.Battle:
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
        FadeInOutHandler fadeInOutHandler = fadeImage.GetComponent<FadeInOutHandler>();

        // 等待淡入结束
        bool isFadeInComplete = false;
        fadeInOutHandler.FadeIn(() => { isFadeInComplete = true; });
        yield return new WaitUntil(() => isFadeInComplete);

        yield return currentLoadedScene.sceneReference.UnLoadScene();

        sceneUnloadCompleteEvent.RaiseEvent(sceneToLoad, this);
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadedScene = sceneToLoad;
        StartCoroutine(FadeOutCoroutine());
        sceneLoadCompleteEvent.RaiseEvent(currentLoadedScene, this);
    }

    private IEnumerator FadeOutCoroutine()
    {
        FadeInOutHandler fadeInOutHandler = fadeImage.GetComponent<FadeInOutHandler>();

        // 等待淡出结束
        bool isFadeOutComplete = false;
        fadeInOutHandler.FadeOut(() => { isFadeOutComplete = true; });
        yield return new WaitUntil(() => isFadeOutComplete);
    }
}