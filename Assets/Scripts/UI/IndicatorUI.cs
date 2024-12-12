using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Indicator : MonoBehaviour
{
    public static Indicator main;
    public TextMeshPro yangCount, yinCount;
    public SpriteRenderer image;
    private int yang, yin;
    public ObjectEventSO switchEvent;
    public bool isDay;
    private bool isRotating;

    void Awake()
    {
        if (main) Destroy(gameObject);
        else main = this;
        yang = 0;
        yin = 0;
    }

    // 监听sceneLoadCompleteEvent
    public void ResetIndicator()
    {
        if (!isDay)
        {
            isDay = true;
            SwitchEvent();
        }
        yang = 0;
        yin = 0;
        UpdateIndicator();
    }

    public void UpdateYinCount()
    {
        yin++;
        UpdateIndicator();
        CheckSwitch();
    }

    public void UpdateYangCount()
    {
        yang++;
        UpdateIndicator();
        CheckSwitch();
    }

    private void UpdateIndicator()
    {
        yangCount.text = yang.ToString();
        yinCount.text = yin.ToString();
    }

    private void CheckSwitch()
    {
        if (yang < yin && isDay)
        {
            isDay = false;
            SwitchEvent();
        }
        else if (yang > yin && !isDay)
        {
            isDay = true;
            SwitchEvent();
        }
    }

    [ContextMenu("SwitchEvent")]
    public void SwitchEvent()
    {
        switchEvent.RaiseEvent(null, this);
        RotateImage();
    }

    private void RotateImage()
    {
        if (isRotating) return;
        Quaternion targetRotation = image.transform.rotation * Quaternion.Euler(0, 0, 180);
        image.transform.DORotateQuaternion(targetRotation, 1).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            isRotating = false;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}