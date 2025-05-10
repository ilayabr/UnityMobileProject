using UnityEngine;
using System.Collections.Generic;

public class ShipPool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();
    public GameObject easyShipPrefab;
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
            GameObject obj = GetFromPool();
            obj.SetActive(true);
        }
    }

    private void InitializePool(int poolSize = 10)
    {
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(easyShipPrefab);
            easyShipPrefab.SetActive(false);
        }
    }

    public GameObject GetFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        return null;
    }

    public void ReturnToPool(GameObject returningObj)
    {
        returningObj.SetActive(false);
    }
}