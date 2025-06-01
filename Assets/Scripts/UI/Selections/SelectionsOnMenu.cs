using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionsOnMenu : MonoBehaviour
{
    public GameObject producerList;
    public GameObject plCloseButton;
    public GameSceneSO firstSceneToLoad;
    public ObjectEventSO loadSceneEvent;
    private PauseManager pauseManager;

    private void Awake()
    {
        pauseManager = PauseManager.Instance;
    }

    #region Event Listening

    public void StartGame()
    {
        loadSceneEvent.RaiseEvent(firstSceneToLoad, "");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenProducerList()
    {
        producerList.SetActive(true);
        pauseManager.PauseGame(new List<Button> { plCloseButton.GetComponent<Button>() });
    }

    public void CloseProducerList()
    {
        pauseManager.ResumeGame();
        producerList.SetActive(false);
    }

    #endregion
}