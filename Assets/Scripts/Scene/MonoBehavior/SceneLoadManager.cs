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
    public List<GameSceneSO> battleScenes;  // 在inspector中添加battleScenes

    public GameObject fadeImage;
    private int currentLevel = 0;

    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;

    [Header("Broadcast Events")]
    public ObjectEventSO sceneUnloadCompleteEvent;
    public ObjectEventSO sceneLoadCompleteEvent;

    private void Start()
    {
        fadeImage.SetActive(true);

        // 刚开始加载菜单场景
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
        fadeInOutHandler.FadeIn();
        yield return new WaitForSeconds(fadeInOutHandler.fadeDuration);
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
        fadeInOutHandler.FadeOut();
        yield return new WaitForSeconds(fadeInOutHandler.fadeDuration);
        fadeImage.SetActive(false);
    }
}
