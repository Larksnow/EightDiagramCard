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
        // Iterate over the keys of the SortedList in sorted order
        for (int i = 0; i < listeners.Keys.Count; i++)
        {
            int key = listeners.Keys[i]; // Get the current priority key
            List<UnityAction<T>> currentListeners = listeners[key]; // Get the listeners for the current priority
            // Iterate over the listeners for the current priority
            for (int j = 0; j < currentListeners.Count; j++)
            {
                currentListeners[j]?.Invoke(data);
            }
        }
        // Update the last sender
        lastSender = sender.ToString();
    }
}