using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class FadablePanel : MonoBehaviour
{
    protected PauseManager pauseManager;
    protected FadeInOutHander fadeInOutHander;
    protected List<Button> buttons = new();

    private ButtonsManager buttonsManager;

    protected virtual void Awake()
    {
        pauseManager = PauseManager.Instance;
        buttonsManager = ButtonsManager.Instance;
        GetFadeInOutHander();
        GetComponentsInChildren(transform, buttons);
    }

    protected virtual void OnEnable()
    {
        fadeInOutHander.FadeIn();
        pauseManager.PauseGame(buttons);
    }

    protected virtual void OnDisable()
    {
        pauseManager.ResumeGame();
    }

    #region Event Listening
    /// <summary>
    /// 监听点击事件，判断点击的物体是否是子物体
    /// </summary>
    /// <param name="obj"></param>
    public void OnClick(object obj)
    {
        PointerEventData pointerEventData = obj as PointerEventData;
        GameObject selected = pointerEventData.pointerPress;
        if (selected.transform.IsChildOf(transform))
        {
            OnClickSelected(selected);
        }
    }
    #endregion

    /// <summary>
    /// 处理点击了选中的Button后的逻辑
    /// </summary>
    /// <param name="selected">选择的Button</param>
    protected virtual void OnClickSelected(GameObject selected)
    {
        // 淡出时先禁用按钮可交互状态，避免重复点击，淡出后再启用
        buttonsManager.SetButtonsInteractable(buttons, false);
        fadeInOutHander.FadeOut(() =>
        {
            gameObject.SetActive(false);
        });
    }

    #region 初始化
    private void GetFadeInOutHander()
    {
        fadeInOutHander = GetComponent<FadeInOutHander>();
        if (fadeInOutHander == null)
        {
            fadeInOutHander = gameObject.AddComponent<FadeInOutHander>();
        }
    }
    #endregion
}