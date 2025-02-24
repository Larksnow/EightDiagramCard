using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitialLoad : MonoBehaviour
{
    public AssetReference persistentScene;

    private void Awake()
    {
        Addressables.LoadSceneAsync(persistentScene);
    }
}