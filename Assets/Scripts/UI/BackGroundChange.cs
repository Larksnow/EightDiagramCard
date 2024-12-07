using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackGroundChange : MonoBehaviour
{
    public SpriteRenderer day;
    public SpriteRenderer night;

    [ContextMenu("TestSwitch")]
    public void SwitchBackround()
    {
        if (Indicator.main.isDay)
        {
            day.DOFade(1, 0.8f);
        }else
        {
            day.DOFade(0, 0.8f);
        }
    }
}
