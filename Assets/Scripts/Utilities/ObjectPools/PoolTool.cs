using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-100)]
public class PoolTool : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject cardPreviewPrefab;
    public GameObject cardPositionPrefab;

    private Dictionary<string, ObjectPool<GameObject>> pools = new();

    // Start is called before the first frame update
    private void Awake()
    {
        var cardPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(cardPrefab, transform),
            actionOnGet: (obj) => obj.SetActive(value: true),
            actionOnRelease: (obj) => obj.SetActive(value: false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
        pools.Add("Card", cardPool);
        var cardPreviewPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(cardPreviewPrefab, transform),
            actionOnGet: (obj) => obj.SetActive(value: true),
            actionOnRelease: (obj) => obj.SetActive(value: false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
        pools.Add("CardPreview", cardPreviewPool);
        PreFillPool(count: 10);
    }

    private void PreFillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            foreach (var pool in pools.Values)
            {
                var obj = pool.Get();
                pool.Release(obj);
            }
        }
    }

    public GameObject GetObjectFromPool(string tag)
    {
        return pools[tag].Get();
    }

    public void ReleaseObjectToPool(string tag, GameObject obj)
    {
        if (pools.ContainsKey(tag))
        {
            pools[tag].Release(obj);
        }
        else
        {
            Debug.LogError($"Pool with tag {tag} does not exist.");
        }
    }
}