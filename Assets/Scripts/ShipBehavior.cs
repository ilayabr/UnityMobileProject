using UnityEngine;

public class ShipBehavior : MonoBehaviour, IPoolable, IHitable
{
    private ShipProperties myProperties;

    [SerializeField] private SpriteRenderer sr;

    private float randomJammerVal;
    private float randomJammerOffset;

    private float speed = 1;

    public ShellTypes shellType;

    public GameObject mainObject
    {
        get => gameObject;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!gameObject.activeInHierarchy) return;

        transform.Translate(Vector3.down * Time.deltaTime * speed);
        if (transform.position.y < -5f)
        {
            GameplayManager.Get().shipPool.ReturnToPool(this);
        }
    }

    public bool IsArtileryCorrect(ShellTypes artileryUsed)
    {
        if (myProperties.difficulty is ShipProperties.Difficulties.JammerOnly 
            or ShipProperties.Difficulties.Basic)
            return true; // no artilery needed
        return shellType == artileryUsed;
    }

    public bool IsJammerSet(float jammerValue)
    {
        if (myProperties.difficulty is ShipProperties.Difficulties.ShellOnly 
            or ShipProperties.Difficulties.Basic)
            return true; // no jammer needed
        return randomJammerVal > jammerValue - randomJammerOffset && randomJammerVal < jammerValue + randomJammerOffset;
    }

    public void SetShipProperties(ShipProperties properties)
    {
        randomJammerVal = properties.jammerValues.GetRandom();
        randomJammerOffset = properties.jammerOffset.GetRandom();
        sr.sprite = properties.sprite;
        speed = properties.speed.GetRandom();
        shellType = (ShellTypes)Random.Range(0,
            System.Enum.GetValues(typeof(ShellTypes)).Length);

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

        gameObject.name =
            $"{System.Enum.GetName(typeof(ShipProperties.Difficulties), myProperties.difficulty)} ship (dead)";
    }

    public bool OnHit(ShellTypes sheelUSed, float jammerVal)
    {
        if (!IsArtileryCorrect(sheelUSed) || !IsJammerSet(jammerVal)) return false;

        GameplayManager.Get().shipPool.ReturnToPool(this);
        GameplayManager.Get().ChangeScore(myProperties.scoreWorth * 100);
        GameplayManager.Get().ChangeMoney(myProperties.value * 1.2f, true);
        AudioManager.Get().PlaySFX(myProperties.exploadSound);

        return true;
    }
}