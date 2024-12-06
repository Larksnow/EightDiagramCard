using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionsOnMenu : MonoBehaviour, ButtonClickHandler
{
    public GameObject startButton;
    public GameObject quitButton;
    public GameObject producerListButton;
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
    public void OnClick(object obj)
    {
        PointerEventData pointerEventData = (PointerEventData)obj;
        GameObject selected = pointerEventData.pointerPress;
        if (selected == startButton)
        {
            loadSceneEvent.RaiseEvent(firstSceneToLoad, "");
        }
        else if (selected == quitButton)
        {
            Application.Quit();
        }
        else if (selected == producerListButton)
        {
            producerList.SetActive(true);
            pauseManager.PauseGame(new List<Button> { plCloseButton.GetComponent<Button>() });
        }
        else if (selected == plCloseButton)
        {
            pauseManager.ResumeGame();
            producerList.SetActive(false);
        }
    }
    #endregion
}