using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHoverScalable
{
    static float tweenDuration = 0.1f;
    void OnHoverEnter(GameObject obj, Vector3 scaleOnEnter)
    {
        obj.transform.DOScale(scaleOnEnter, tweenDuration).SetEase(Ease.OutExpo);
    }
    
    void OnHoverExit(GameObject obj, Vector3 scaleOnExit)
    {
        obj.transform.DOScale(scaleOnExit, tweenDuration).SetEase(Ease.OutExpo);
    }
}