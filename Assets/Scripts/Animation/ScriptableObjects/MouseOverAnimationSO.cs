using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIHoverAnimation", menuName = "Animation/UIHoverAnimation")]
public class MouseOverAnimationSO : ScriptableObject
{
    public Ease easeOnEnter;
    public Ease easeOnExit;
    public float durationOnEnter;
    public float durationOnExit;
}