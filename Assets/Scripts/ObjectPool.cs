using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    private List<IPoolable> pool = new();
    public GameObject prefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ActivateFromPool();
        }
    }

    public void InitializePool(int poolSize = 10)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            IPoolable poolable = obj.GetComponent<IPoolable>();
            pool.Add(poolable);
            poolable.OnEnterPool();
            poolable.mainObject.SetActive(false);
        }
    }

    public GameObject ActivateFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].mainObject.activeInHierarchy)
            {
                pool[i].mainObject.SetActive(true);
                pool[i].OnExitPool();
                return pool[i].mainObject;
            }
        }

        return null;
    }

    public void ReturnToPool(IPoolable returningObj)
    {
        returningObj.OnEnterPool();
        returningObj.mainObject.SetActive(false);
    }
}