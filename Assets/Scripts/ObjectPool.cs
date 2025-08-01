using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System.Linq;
using System;

/// <summary>
/// a generic pool class that can contain any IPoolable
/// </summary>
public class ObjectPool : MonoBehaviour
{
    private readonly List<IPoolable> pool = new();

    public event Action<IPoolable> OnObjectDeactivated;

    /// <summary>
    /// <para>Creates a number of instances of a prefab within this pool.</para> 
    /// The prefab MUST have a component of type 'IPoolable' within its root object!
    /// </summary>
    /// <param name="prefab">The prebaf to spawn into the pool</param>
    /// <param name="poolSize">The amount of the given prefab to spawn into the pool</param>
    public void InitializePool(GameObject prefab, int poolSize = 10)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);

            if (!obj.TryGetComponent(out IPoolable poolable))
            {
                Destroy(obj);
                Debug.LogWarning($"Attempt to initialize pool '{gameObject.name}' failed! given prefabs ('{prefab.name}') root object does not contain a component of type 'IPoolable'!");
                return;
            }

            pool.Add(poolable);
            poolable.OnEnterPool();
            poolable.mainObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activates the first inactive object within the pool
    /// </summary>
    /// <returns>The activated object</returns>
    public GameObject ActivateFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].mainObject.activeInHierarchy) continue;

            pool[i].mainObject.SetActive(true);
            pool[i].OnExitPool();
            return pool[i].mainObject;
        }

        return null;
    }

    /// <summary>
    /// Returns the given object back into the pull, setting it inactive and calling its 'OnEnterPool()' method
    /// </summary>
    /// <param name="returningObj">The object to return into the pool</param>
    public void ReturnToPool(IPoolable returningObj)
    {
        if (!returningObj.mainObject.activeInHierarchy) return;

        returningObj.mainObject.SetActive(false);
        returningObj.OnReturnedToPool();

        OnObjectDeactivated?.Invoke(returningObj);
    }

    public List<IPoolable> GetLivingPoolables() => pool.Where(e => e.mainObject.activeSelf).ToList();
}