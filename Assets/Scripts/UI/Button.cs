using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// 通用按钮组件, 使用方法：
/// 一、非Canvas渲染的按钮：
/// 1. 在编辑器中创建一个空物体，添加Button脚本组件，在Button Settings中设置interactable等相关属性
/// 2. 创建并调整对应的BoxCollider2D组件
/// 3. 向OnClick(含参数GameObject)中添加需要执行的函数
///
/// 二、Canvas渲染的按钮：
/// 1. 在编辑器中创建一个UI Button，添加Button脚本组件
/// 2. 不需要创建BoxCollider2D组件即可使用
/// </summary>
public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
    IPointerClickHandler, IColliderSetUp, IScaleOnMouseOver
{
    [Serializable]
    private class ButtonSettings
    {
        public bool interactable = true;
        public float scaleOnEnter = 1.1f;
        public float scaleOnClick = 1.15f;
    }

    [SerializeField] private ButtonSettings settings;

    public UnityEvent<GameObject> OnClick;
    public MouseOverAnimationSO mouseOverAnimationSO; // 鼠标进入/退出该按钮时的缓动效果

    private Vector3 originalScale;
    private ButtonsManager buttonsManager;

    private void Awake()
    {
        originalScale = transform.localScale;
        buttonsManager = ButtonsManager.Instance;

        if (GetComponent<BoxCollider2D>() == null)
            ((IColliderSetUp)this).SetupCollider(gameObject);
    }

    private void Update()
    {
        UpdateInteractableState();
    }

    private void OnEnable()
    {
        buttonsManager.AddButton(this);
    }

    private void OnDisable()
    {
        buttonsManager.RemoveButton(this);
    }

    private void UpdateInteractableState()
    {
        settings.interactable = buttonsManager.IsButtonInteractable(this);
    }

    #region Pointer Event Handlers

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!settings.interactable) return;
        ((IScaleOnMouseOver)this).OnMouseEnter(gameObject, settings.scaleOnEnter * transform.localScale,
            mouseOverAnimationSO);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!settings.interactable) return;
        ((IScaleOnMouseOver)this).OnMouseExit(gameObject, originalScale, mouseOverAnimationSO);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!settings.interactable) return;
        ((IScaleOnMouseOver)this).OnMouseEnter(gameObject, settings.scaleOnClick * transform.localScale,
            mouseOverAnimationSO);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!settings.interactable) return;

        ((IScaleOnMouseOver)this).OnMouseExit(gameObject, originalScale, mouseOverAnimationSO);
        OnClick.Invoke(gameObject);
    }

    #endregion
}