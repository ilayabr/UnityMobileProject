using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private ShipProperties basicShipProperties;
    [SerializeField] private ShipProperties ShellShipProperties;
    [SerializeField] private ShipProperties JammerShipProperties;
    [SerializeField] private ShipProperties ShellAndJammerShipProperties;

    [FormerlySerializedAs("objectPool")] public ObjectPool shipPool;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text timeText;
    private SaveData GM;
    public int Score => GM.score;
    public float Money => GM.money;

    private int textDotCount;
    private TimeSpan timePlayed;
    public TimeSpan TimePlayed{
        get => timePlayed;
        set
        {
            timePlayed = value;
            Debug.Log($"Time played updated: {timePlayed:mm\\:ss\\.ff}");
        }
    }
    
    private int lives = 3;
    public int Lives
    {
        get => lives;
        set
        {
            lives = value;
            Debug.Log($"Lives updated: {lives}");
        }
    }
    protected override bool DontDestroyOnLoad => false;


    public void ChangeScore(int points)
    {
        GM.score += points;
        textDotCount = 10 - GM.score.ToString().Length;

        string dots = "";
        for (int i = 0; i < textDotCount; i++)
        {
            dots += ".";
        }

        scoreText.text = $"pts{dots}{GM.score}";
        Debug.Log($"Score updated: {GM.score}");
    }

    public void ChangeMoney(float amount, bool positive)
    {
        GM.money += positive ? amount : -amount;
        textDotCount = 9 - GM.money.ToString("0.0").Length;

        string dots = "";
        for (int i = 0; i < textDotCount; i++)
        {
            dots += ".";
        }

        moneyText.text = $"sal{dots}{GM.money:F1}$";
        Debug.Log($"Money updated: {GM.money}");
    }


    private void Start()
    {
        GM = GameManager.Get().currentSaveData;
        shipPool.InitializePool(shipPrefab, 20);
        StartCoroutine(SpawnShips());
    }

    void Update()
    {
        ClickHandler();
        timePlayed += TimeSpan.FromSeconds(Time.deltaTime);
        timeText.text = $"{timePlayed:mm\\:ss\\.ff}";
        GM.timePlayed = timePlayed.TotalSeconds;
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
    void ClickHandler()
    {
        if (!Input.GetMouseButtonDown(0) || pauseMenu.IsPaused) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<RaycastHit2D> rayHits = new();

        if (Physics2D.Raycast(mouseWorldPos, Vector2.zero, filter, rayHits) == 0) return;

        if (!rayHits[0].transform.TryGetComponent(out IHitable hitObject)) return;

        hitObject.OnHit();
    }

    public void PauseGame()
    {
        pauseMenu.SetPauseState(true);
    }
}