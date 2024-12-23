using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// 通用按钮组件, 使用方法：
/// 一、非Canvas渲染的按钮：
/// 1. 在编辑器中创建一个空物体，添加Button脚本组件，在Button Settings中设置interactable等相关属性
/// 2. 创建并调整对应的BoxCollider2D组件
/// 3. 绑定固定的点击事件`OnClickedEvent`（处理全局点击的事件）
/// 4. 在Button物体所在的父物体（Panel）中添加监听函数`OnClick`，并在Panel添加`ObjectListener`组件，对`OnClickedEvent`进行监听
///
/// 二、Canvas渲染的按钮：
/// 1. 在编辑器中创建一个UI Button，添加Button脚本组件
/// 2. 不需要创建BoxCollider2D组件即可使用
/// </summary>
public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
    IPointerClickHandler, IColliderSetUp, IHoverScalable
{
    [Serializable]
    private class ButtonSettings
    {
        public bool interactable = true;
        public float scaleOnEnter = 1.1f;
        public float scaleOnClick = 1.15f;
    }

    [SerializeField] private ButtonSettings settings;
    [SerializeField] private ObjectEventSO onClickedEvent;

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
        ((IHoverScalable)this).OnHoverEnter(gameObject, settings.scaleOnEnter * transform.localScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!settings.interactable) return;
        ((IHoverScalable)this).OnHoverExit(gameObject, originalScale);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!settings.interactable) return;
        ((IHoverScalable)this).OnHoverEnter(gameObject, settings.scaleOnClick * transform.localScale);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!settings.interactable) return;

        ((IHoverScalable)this).OnHoverExit(gameObject, originalScale);
        onClickedEvent?.RaiseEvent(eventData, this);
    }

    #endregion
}