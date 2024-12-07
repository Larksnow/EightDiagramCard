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
    public List<GameObject> persistentObjectsInBattle; // 在战斗场景中持久化的object，无需重复加载，且在菜单场景中先设为inactive
    private int currentLevel = 0;

    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;

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
                SetBattlePersistentObjectsActive(false);
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
        FadeInOutHander fadeInOutHander = fadeImage.GetComponent<FadeInOutHander>();
        fadeInOutHander.FadeIn();
        yield return new WaitForSeconds(fadeInOutHander.fadeDuration);
        yield return currentLoadedScene.sceneReference.UnLoadScene();
        if (currentLoadedScene.sceneType == SceneType.Menu)
        {
            SetBattlePersistentObjectsActive(true);
        }
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
    }

    private IEnumerator FadeOutCoroutine()
    {
        FadeInOutHander fadeInOutHander = fadeImage.GetComponent<FadeInOutHander>();
        fadeInOutHander.FadeOut();
        yield return new WaitForSeconds(fadeInOutHander.fadeDuration);
        fadeImage.SetActive(false);
    }

    private void SetBattlePersistentObjectsActive(bool isActive)
    {
        foreach (GameObject obj in persistentObjectsInBattle)
        {
            obj.SetActive(isActive);
        }
    }
}
