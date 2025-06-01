using TMPro;
using UnityEngine;
using UnityEngine.Pool;
[DefaultExecutionOrder(-100)]
public class DamageNumberPool : MonoBehaviour
{
    public GameObject damageNumberPrefab;

    private ObjectPool<GameObject> pool;
    private Color originalColor;
    private Vector3 originScale;
    private Vector3 originPosition;

    private void Awake()
    {
        originalColor = damageNumberPrefab.GetComponent<TextMeshPro>().color;
        originScale = damageNumberPrefab.transform.localScale;
        originPosition = damageNumberPrefab.transform.position;

        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(damageNumberPrefab, transform),
            actionOnGet: (obj) => obj.SetActive(value: true),
            actionOnRelease: (obj) =>
            {
                obj.SetActive(value: false);
                obj.GetComponent<TextMeshPro>().color = originalColor;
                obj.transform.localScale = originScale;
                obj.transform.position = originPosition;
            },
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
        PreFillPool(count: 10);
    }

    private void PreFillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = pool.Get();
            pool.Release(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    public void ReleaseObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
