using UnityEngine;

public class ShipBehavior : MonoBehaviour, IPoolable, IHitable
{
    private ShipProperties myProperties;

    [SerializeField] private SpriteRenderer sr;

    private float randomJammerVal;

    private float speed = 1;

    private ShipProperties.ShellTypes shellType;

    public GameObject mainObject { get => gameObject; }
    
    void Update()
    {
        Movement();
    }

    void Movement(){
        if (!gameObject.activeInHierarchy) return;

        transform.Translate(Vector3.down * Time.deltaTime * speed);
        if (transform.position.y < -5f)
        {
            GameManager.instance.shipPool.ReturnToPool(this);
        }
    }

    public bool IsArtileryCorrect(ShipProperties.ShellTypes artileryUsed)
    {
        if (myProperties.difficulty == ShipProperties.Difficulties.JammerOnly ||
            myProperties.difficulty == ShipProperties.Difficulties.Basic)
            return true;
        if (shellType == artileryUsed)
            return true;
        return false;
    }

    public bool IsJammerSet(float jammerValue)
    {
        if (myProperties.difficulty == ShipProperties.Difficulties.ShellOnly ||
            myProperties.difficulty == ShipProperties.Difficulties.Basic)
            return true;
        if (randomJammerVal > jammerValue - 2.5f && randomJammerVal < jammerValue + 2.5f)
            return true;

        return false;
    }

    public void SetShipProperties(ShipProperties properties){
        randomJammerVal = properties.jammerValues.GetRandom();
        sr.sprite = properties.sprite;
        speed = properties.speed.GetRandom();
        shellType = (ShipProperties.ShellTypes)Random.Range(0, System.Enum.GetValues(typeof(ShipProperties.ShellTypes)).Length);

        myProperties = properties;

        gameObject.name = $"{System.Enum.GetName(typeof(ShipProperties.Difficulties), myProperties.difficulty)} ship";
    }

    public void OnEnterPool()
    {
        
    }

    public void OnExitPool()
    {
        var pos = transform.position;
        pos.x = Random.Range(-8f, 8f);
        pos.y = 5;
        transform.position = pos;

        if (myProperties == null) return;

        gameObject.name = $"{System.Enum.GetName(typeof(ShipProperties.Difficulties), myProperties.difficulty)} ship";
    }

    public void OnReturnedToPool()
    {
        if (myProperties == null) return;

        gameObject.name = $"{System.Enum.GetName(typeof(ShipProperties.Difficulties), myProperties.difficulty)} ship (dead)";
    }

    public void OnHit()
    {
        GameManager.instance.shipPool.ReturnToPool(this);
    }
}