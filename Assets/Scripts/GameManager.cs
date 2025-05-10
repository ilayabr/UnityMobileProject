using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ShipProperties basicShipProperties;
    public ShipProperties ShellShipProperties;
    public ShipProperties JammerShipProperties;
    public ShipProperties ShellAndJammerShipProperties;
    
    [FormerlySerializedAs("objectPool")] public ObjectPool shipPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        shipPool.InitializePool();
        StartCoroutine(SpawnShips());
    }
    
    private IEnumerator SpawnShips()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SpawnShip();
        }
    }

    public void SpawnShip()
    {
        GameObject ship = shipPool.ActivateFromPool();
        if (!ship) return;
        ShipBehavior shipBehavior = ship.GetComponent<ShipBehavior>();
        shipBehavior.SetShipProperties(basicShipProperties);
    }
    
    
}