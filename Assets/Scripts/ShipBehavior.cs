using TMPro;
using UnityEngine;

public class ShipBehavior : MonoBehaviour, IPoolable, IHitable
{
    public ShipProperties MyProperties { get; private set; }

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private ParticleSystem particles;

    public float RandomJammerVal { get; private set; }
    public float RandomJammerOffset { get; private set; }

    private float speed = 1;

    public ShellTypes shellType;

    public string shipNameID;

    public GameObject mainObject
    {
        get => gameObject;
    }

    void Start()
    {
        particles.gameObject.transform.SetParent(transform.parent);
    }

    void Update()
    {
        Movement();

        particles.transform.position = transform.position;
    }

    void Movement()
    {
        if (!gameObject.activeInHierarchy) return;

        transform.Translate(Vector3.down * Time.deltaTime * speed);
        if (transform.position.y < -5f)
        {
            GameplayManager.Get().ChangeLives(-1);
            Handheld.Vibrate();
            AudioManager.Get().PlaySFX(MyProperties.exploadSound);
            GameplayManager.Get().shipPool.ReturnToPool(this);
        }
    }

    public bool IsArtileryCorrect(ShellTypes artileryUsed)
    {
        if (MyProperties.difficulty is ShipProperties.Difficulties.JammerOnly 
            or ShipProperties.Difficulties.Basic)
            return true; // no artilery needed
        return shellType == artileryUsed;
    }

    public bool IsJammerSet(float jammerValue)
    {
        if (MyProperties.difficulty is ShipProperties.Difficulties.ShellOnly 
            or ShipProperties.Difficulties.Basic)
            return true; // no jammer needed
        return RandomJammerVal > jammerValue - RandomJammerOffset && RandomJammerVal < jammerValue + RandomJammerOffset;
    }

    public void SetShipProperties(ShipProperties properties)
    {
        RandomJammerVal = properties.jammerValues.GetRandom();
        RandomJammerOffset = properties.jammerOffset.GetRandom();
        sr.sprite = properties.sprite;
        speed = properties.speed.GetRandom();
        shellType = (ShellTypes)Random.Range(0,
            System.Enum.GetValues(typeof(ShellTypes)).Length);

        GameplayManager.Get().CreateNameIDForShip(this);

        MyProperties = properties;

        nameText.text = shipNameID;
        gameObject.name = $"{MyProperties.name + nameText.text} ship";
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

        if (MyProperties == null) return;

        gameObject.name = $"{System.Enum.GetName(typeof(ShipProperties.Difficulties), MyProperties.difficulty)} ship";
    }

    public void OnReturnedToPool()
    {
        if (MyProperties == null) return;

        gameObject.name =
            $"{System.Enum.GetName(typeof(ShipProperties.Difficulties), MyProperties.difficulty)} ship (dead)";
        
        shellType = Random.Range(0,
            System.Enum.GetValues(typeof(ShellTypes)).Length) switch
        {
            0 => ShellTypes.HE,
            1 => ShellTypes.AP,
            2 => ShellTypes.AT,
            _ => ShellTypes.HE // default to HE if something goes wrong
        };
    }

    public bool OnHit(ShellTypes sheelUSed, float jammerVal)
    {
        if (!IsArtileryCorrect(sheelUSed) || !IsJammerSet(jammerVal)) return false;

        GameplayManager.Get().ChangeScore(MyProperties.scoreWorth * (int)transform.position.y);
        GameplayManager.Get().ChangeMoney(MyProperties.value * 1.2f, true);
        GameplayManager.Get().shipPool.ReturnToPool(this);
        AudioManager.Get().PlaySFX(MyProperties.exploadSound);

        return true;
    }
}