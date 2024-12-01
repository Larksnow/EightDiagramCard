using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource day;
    public AudioSource night;
    public void SwitchBGM()
    {
        if (Indicator.main.isDay)
        {
            night.DOFade(0, 5f).SetEase(Ease.InOutSine);
            day.DOFade(1, 5f).SetEase(Ease.InOutSine);
        }else
        {
            night.DOFade(1, 0.8f).SetEase(Ease.InOutSine);
            day.DOFade(0, 5f).SetEase(Ease.InOutSine);
        }
    }
}
