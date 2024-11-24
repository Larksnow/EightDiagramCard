using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO;
    public UnityEvent<T> response;
    
    // This priority is used to determine the order of response when multiple listeners are registered to the same event.
    // Lower value of priority will be executed first.
    public int priority = 0;

    private void OnEnable()
    {
        if(eventSO!=null)
        {
            eventSO.RegisterListener(OnEventRaised, priority);
        }
    }

    private void OnDisable()
    {
        if (eventSO != null)
        {
            eventSO.UnregisterListener(OnEventRaised);
        }
    }

    private void OnEventRaised(T value)
    {
        response.Invoke(value);
    }
}
