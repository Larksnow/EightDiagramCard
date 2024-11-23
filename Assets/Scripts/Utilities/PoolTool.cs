using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool; 
    // Start is called before the first frame update
    private void Start()
    {
        pool = new ObjectPool<GameObject>(
            createFunc:()=>Instantiate(objPrefab, transform),
            actionOnGet:(obj)=>obj.SetActive(value: true),
            actionOnRelease:(obj)=>obj.SetActive(value: false),
            actionOnDestroy:(obj)=>Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
        PreFillPool(count:10);
    }

    private void PreFillPool(int count)
    {
        var PreFillArray = new GameObject[count];
        for(int i = 0; i < count; i++)
        {
            PreFillArray[i] = pool.Get();
        }
        foreach (var item in PreFillArray)
        {
            pool.Release(item);
        }
    }

    public GameObject GetObjectFromPool(){
        return pool.Get();
    } 

    public void ReleaseObjectToPool(GameObject obj){
        pool.Release(obj);
    }
}
