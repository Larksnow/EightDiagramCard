using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [Header("Scenes")] public GameSceneSO testScene;
    public GameSceneSO menuScene;
    public List<GameSceneSO> battleScenes; // 在inspector中添加battleScenes

    public GameObject fadeImage;
    private int currentLevel = 0;

    [SerializeField] private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;

    [Header("Broadcast Events")] public ObjectEventSO sceneUnloadCompleteEvent;
    public ObjectEventSO sceneLoadCompleteEvent;

    private void Start()
    {
        // fadeImage.SetActive(true);

        // // 刚开始加载菜单场景
        // OnLoadRequest(menuScene);
        currentLoadedScene = testScene;
        sceneLoadCompleteEvent.RaiseEvent(currentLoadedScene, this);
    }

    [ContextMenu("LoadMenu")]
    public void LoadMenu()
    {
        OnLoadRequest(menuScene);
    }

    #region Event Listening

    public void NextLevel()
    {
        if (battleScenes.Count < currentLevel + 1)
        {
            Debug.LogError("No more levels to load");
            return;
        }

        OnLoadRequest(battleScenes[++currentLevel]);
    }

    public void OnLoadRequest(object obj)
    {
        GameSceneSO gameSceneToGo = (GameSceneSO)obj;
        switch (gameSceneToGo.sceneType)
        {
            case SceneType.Menu:
                sceneToLoad = menuScene;
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
        fadeImage.SetActive(true);
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

        fadeImage.SetActive(false);
    }
}