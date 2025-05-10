using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ShipPool : MonoBehaviour
{
    private List<IPoolable> pool = new();
    public GameObject shipPrefab;
    public static ShipPool instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializePool();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            IPoolable obj = GetFromPool();
            obj.mainObject.SetActive(true);
            obj.OnExitPool();
        }
    }

    private void InitializePool(int poolSize = 10)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ship = Instantiate(shipPrefab, transform);
            IPoolable poolable = ship.GetComponent<IPoolable>();
            pool.Add(poolable);
            poolable.OnEnterPool();
            poolable.mainObject.SetActive(false);
        }
    }

    public IPoolable GetFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].mainObject.activeInHierarchy)
            {
                return pool[i];
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