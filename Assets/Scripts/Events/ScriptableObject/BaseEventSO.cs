using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;

    public UnityAction<T> OnEventRaised;
    public string lastSender;
    private SortedList<int, List<UnityAction<T>>> listeners = new SortedList<int, List<UnityAction<T>>>();
    public void RegisterListener(UnityAction<T> listener, int priority = 0)
    {
        if (!listeners.ContainsKey(priority))
        {
            listeners[priority] = new List<UnityAction<T>>();
        }

        listeners[priority].Add(listener);
    }

    public void UnregisterListener(UnityAction<T> listener)
    {
        foreach (var key in listeners.Keys)
        {
            if (listeners[key].Remove(listener) && listeners[key].Count == 0)
            {
                listeners.Remove(key);
                return;
            }
        }
    }

    public void RaiseEvent(T data, object sender)
    {
        foreach (var pair in listeners)
        {
            foreach (var listener in pair.Value)
            {
                listener?.Invoke(data);
            }
        }
        lastSender = sender.ToString();
    }
}