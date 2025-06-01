using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 一类淡入淡出面板的基类
/// 包含一系列含`Button`脚本的可交互按钮子物体
/// 启用时淡入；点击按钮后淡出
///
/// 继承类需实现`OnClickSelected`方法，处理点击按钮后的逻辑
/// </summary>
public abstract class FadablePanel : MonoBehaviour
{
    protected FadeInOutHandler fadeInOutHandler;
    
    private PauseManager pauseManager;
    private List<Button> buttons = new();
    private ButtonsManager buttonsManager;

    protected virtual void Awake()
    {
        pauseManager = PauseManager.Instance;
        buttonsManager = ButtonsManager.Instance;
        GetFadeInOutHandler();
        GetComponentsInChildren(transform, buttons);
    }

    protected virtual void OnEnable()
    {
        fadeInOutHandler.FadeIn();
        pauseManager.PauseGame(buttons);
        Debug.Log("PauseGame");
    }

    protected virtual void OnDisable()
    {
        pauseManager.ResumeGame();
    }

    /// <summary>
    /// 点击面板按钮后淡出
    /// </summary>
    public virtual void FadeOutAfterClick(GameObject selected)
    {
        // 淡出时先禁用按钮可交互状态，避免重复点击，淡出后再启用
        buttonsManager.SetButtonsInteractable(buttons, false);
        fadeInOutHandler.FadeOut(() =>
        {
            gameObject.SetActive(false);
        });
    }

    #region 初始化
    private void GetFadeInOutHandler()
    {
        fadeInOutHandler = GetComponent<FadeInOutHandler>();
        if (fadeInOutHandler == null)
        {
            fadeInOutHandler = gameObject.AddComponent<FadeInOutHandler>();
        }
    }
    #endregion
}