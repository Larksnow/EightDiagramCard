using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    private bool isPaused;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PauseGame()
    {
        isPaused = true;
    }
    public void UnpauseGame()
    {
        isPaused = false;
    }
    public bool IsPaused()
    {
        return isPaused;
    }
}