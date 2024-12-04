using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionsOnMenu : MonoBehaviour, ButtonClickHandler
{
    public GameObject startButton;
    public GameObject quitButton;
    public GameObject producerListButton;
    public GameObject producerList;
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
            else if (selected == producerListButton)
            {
                producerList.SetActive(!producerList.activeSelf);
                // TODO:关闭按钮
            }
        }
    }
    #endregion
}