using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IScaleOnMouseOver
{
    /// <summary>
    /// 鼠标进入/点击时播放该动画
    /// </summary>
    void OnMouseEnter(GameObject obj, Vector3 scaleOnEnter, MouseOverAnimationSO mouseOverAnimationSO)
    {
        obj.transform.DOKill();
        obj.transform.DOScale(scaleOnEnter, mouseOverAnimationSO.durationOnEnter)
            .SetEase(mouseOverAnimationSO.easeOnEnter);
    }

    /// <summary>
    /// 鼠标离开/点击松开时播放该动画
    /// </summary>
    void OnMouseExit(GameObject obj, Vector3 scaleOnExit, MouseOverAnimationSO mouseOverAnimationSO)
    {
        obj.transform.DOKill();
        obj.transform.DOScale(scaleOnExit, mouseOverAnimationSO.durationOnExit)
            .SetEase(mouseOverAnimationSO.easeOnExit);
    }
}