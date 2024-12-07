using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ButtonsManager : MonoBehaviour
{
    public static ButtonsManager Instance { get; private set; }
    private HashSet<Button> interactableButtons = new();
    private HashSet<Button> buttons = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #region 可交互状态管理
    /// <summary>
    /// 管理游戏中所有已启用按钮的可交互状态
    /// 不建议非Manager类调用
    /// </summary>
    /// <param name="interactable"></param>
    public void SetAllButtonsInteractable(bool interactable)
    {
        foreach (var button in this.buttons)
        {
            UpdateButtonInteractableState(button, interactable);
        }
    }

    public void SetButtonsInteractable(List<Button> btns, bool interactable)
    {
        foreach (var button in btns)
        {
            UpdateButtonInteractableState(button, interactable);
        }
    }

    public void SetButtonInteractable(Button btns, bool interactable)
    {
        UpdateButtonInteractableState(btns, interactable);
    }

    public bool IsButtonInteractable(Button button)
    {
        return interactableButtons.Contains(button);
    }

    private void UpdateButtonInteractableState(Button btn, bool interactable)
    {
        if (interactable)
        {
            interactableButtons.Add(btn);
        }
        else
        {
            interactableButtons.Remove(btn);
        }
    }
    #endregion


    #region 生命周期管理
    public void AddButton(Button button)
    {
        buttons.Add(button);
        interactableButtons.Add(button); // 默认为可交互
    }
    public void RemoveButton(Button button)
    {
        buttons.Remove(button);
        interactableButtons.Remove(button);
    }
    #endregion
}