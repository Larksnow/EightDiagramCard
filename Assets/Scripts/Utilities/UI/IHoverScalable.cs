using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHoverScalable
{
    static float tweenDuration = 0.1f;
    void OnHoverEnter(GameObject obj, Vector3 scaleOnEnter, Ease ease)
    {
        obj.transform.DOKill();
        obj.transform.DOScale(scaleOnEnter, 0.1f).SetEase(ease);
    }
    
    void OnHoverExit(GameObject obj, Vector3 scaleOnExit, Ease ease)
    {
        obj.transform.DOKill();
        obj.transform.DOScale(scaleOnExit, 0.5f).SetEase(ease);
    }
}