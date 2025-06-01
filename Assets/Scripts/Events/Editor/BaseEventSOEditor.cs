using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if (baseEventSO == null)
        {
            baseEventSO = target as BaseEventSO<T>;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var listeners = GetListeners();
        EditorGUILayout.LabelField("Subscribe Number: " + listeners.Count);
        
        if (listeners.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Registered Listeners:", EditorStyles.boldLabel);
            
            foreach (var kvp in listeners)
            {
                EditorGUILayout.LabelField($"Priority {kvp.Key}:");
                EditorGUI.indentLevel++;
                foreach (var listener in kvp.Value)
                {
                    EditorGUILayout.LabelField($"  • {listener}");
                }
                EditorGUI.indentLevel--;
            }
        }
        
        // Show last sender info if available
        if (!string.IsNullOrEmpty(baseEventSO.lastSender))
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Last Sender: " + baseEventSO.lastSender);
        }
    }

    private Dictionary<int, List<MonoBehaviour>> GetListeners()
    {
        Dictionary<int, List<MonoBehaviour>> listeners = new Dictionary<int, List<MonoBehaviour>>();
        
        if (baseEventSO == null)
        {
            return listeners;
        }

        // Use reflection to access the private listeners field
        var listenersField = typeof(BaseEventSO<T>).GetField("listeners", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (listenersField == null)
        {
            return listeners;
        }

        var listenersData = listenersField.GetValue(baseEventSO) as SortedList<int, List<UnityEngine.Events.UnityAction<T>>>;
        
        if (listenersData == null)
        {
            return listeners;
        }

        // Extract MonoBehaviour targets from the registered listeners
        foreach (var kvp in listenersData)
        {
            int priority = kvp.Key;
            var actionList = kvp.Value;
            
            listeners[priority] = new List<MonoBehaviour>();
            
            foreach (var action in actionList)
            {
                if (action?.Target is MonoBehaviour mb && !listeners[priority].Contains(mb))
                {
                    listeners[priority].Add(mb);
                }
            }
        }

        return listeners;
    }
}