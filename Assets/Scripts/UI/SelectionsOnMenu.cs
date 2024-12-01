using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionsOnMenu : MonoBehaviour {
    public GameObject startButton;
    public GameObject quitButton;
    public GameSceneSO firstSceneToLoad;
    public ObjectEventSO loadSceneEvent;

    #region Event Listening
    public void OnClick(object obj)
    {
        PointerEventData pointerEventData = (PointerEventData)obj;
        GameObject selected = pointerEventData.pointerPress;
        if (selected.transform.IsChildOf(transform))
        {
            if (selected == startButton)
            {
                loadSceneEvent.RaiseEvent(firstSceneToLoad, "");
            }
            else if (selected == quitButton)
            {
                Application.Quit();
            }
        }
    }
    #endregion
}