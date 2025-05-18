using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private ShipProperties basicShipProperties;
    [SerializeField] private ShipProperties ShellShipProperties;
    [SerializeField] private ShipProperties JammerShipProperties;
    [SerializeField] private ShipProperties ShellAndJammerShipProperties;
    
    [FormerlySerializedAs("objectPool")] public ObjectPool shipPool;
    [SerializeField] private GameObject shipPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        shipPool.InitializePool(shipPrefab, 20);
        StartCoroutine(SpawnShips());
    }

    void Update()
    {
        ClickHandler();
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
    
    /// <summary>
    /// Creates a raycast when clicking the left mouse button and if an object is found and has the 'IHitable' interface it will call its 'OnHit()' method
    /// </summary>
    void ClickHandler(){
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<RaycastHit2D> rayHits = new();

        if (Physics2D.Raycast(mouseWorldPos, Vector2.zero, filter, rayHits) == 0) return;

        if (!rayHits[0].transform.TryGetComponent(out IHitable hitObject)) return;

        hitObject.OnHit();
    }
}