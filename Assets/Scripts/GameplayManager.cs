using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum ShellTypes
{
    HE,
    AP,
    AT
}

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private WeightedList<ShipProperties> possibleShips;
    [SerializeField] private float shipSpawnSpeed;

    [FormerlySerializedAs("objectPool")] public ObjectPool shipPool;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AudioClip gameOverSfx;
    [SerializeField] private TMP_Text gameOverScore;
    [SerializeField] private TMP_Text gameOverMoney;
    [SerializeField] private TMP_Text gameOverTime;
    private SaveData GM;
    public int Score => GM.score;
    public float Money => GM.money;

    private int textDotCount;
    private TimeSpan timePlayed;
    public TimeSpan TimePlayed
    {
        get => timePlayed;
        set
        {
            timePlayed = value;
            Debug.Log($"Time played updated: {timePlayed:mm\\:ss\\.ff}");
        }
    } 
    
    public int Lives => GM.lives;
    public void ChangeLives(int amount)
    {
        GM.lives += amount;
        textDotCount = 10 - GM.lives.ToString().Length;

        string dots = "";
        for (int i = 0; i < textDotCount; i++)
        {
            dots += ".";
        }
        livesText.text = $"LIV{dots}{GM.lives}";

        if (GM.lives <= 0)
        {
            GameOver();
        }
    }
    protected override bool DontDestroyOnLoad => false;

    private List<ShipBehavior> allShipsAlive => shipPool.GetLivingPoolables().Select(e => e.mainObject.GetComponent<ShipBehavior>()).ToList();

    private Dictionary<string, GameObject> existingShipInfoThings = new();
    [SerializeField] private GameObject shipInfoPrefab;
    [SerializeField] private Transform shipInfoParent;


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
        GM.lives = 3;
        shipPool.InitializePool(shipPrefab, 3);
        shipPool.OnObjectDeactivated += OnShipDisabled;
        StartCoroutine(SpawnShips());
    }

    void Update()
    {
        timePlayed += TimeSpan.FromSeconds(Time.deltaTime);
        timeText.text = $"{timePlayed:mm\\:ss\\.ff}";
        GM.timePlayed = timePlayed.TotalSeconds;
    }

    private IEnumerator SpawnShips()
    {
        while (true)
        {
            yield return new WaitForSeconds(shipSpawnSpeed);
            SpawnShip();
        }
    }

    public void SpawnShip()
    {
        GameObject ship = shipPool.ActivateFromPool();
        if (!ship) return;
        ShipBehavior shipBehavior = ship.GetComponent<ShipBehavior>();
        shipBehavior.SetShipProperties(possibleShips.ChooseRandom());

        var infoPoint = Instantiate(shipInfoPrefab, shipInfoParent);

        infoPoint.GetComponent<ShipInfoCell>().Setup(shipBehavior);

        existingShipInfoThings.Add(shipBehavior.shipNameID, infoPoint);
    }

    public void PauseGame()
    {
        pauseMenu.SetPauseState(true);
    }

    public void CreateNameIDForShip(ShipBehavior ship)
    {
        while (true)
        {
            ship.shipNameID = UnityEngine.Random.Range(0, 100).ToString();

            int amountOfShipsWithTheSameName = 0;

            foreach (var livingShip in allShipsAlive)
            {
                if (livingShip == ship) continue;
                string nameID = livingShip.shipNameID;

                if (!char.IsDigit(livingShip.shipNameID[livingShip.shipNameID.Length - 1]))
                    nameID = livingShip.shipNameID.Substring(0, livingShip.shipNameID.Length - 1);

                if (nameID.Equals(ship.shipNameID))
                    amountOfShipsWithTheSameName++;
            }

            if (amountOfShipsWithTheSameName == 0)
                break;
            else if (amountOfShipsWithTheSameName < 3)
            {
                var extraLetter = (char)(65 + amountOfShipsWithTheSameName);

                ship.shipNameID += extraLetter;

                break;
            }
        }
    }

    public void OnShipDisabled(IPoolable obj)
    {
        var id = obj.mainObject.GetComponent<ShipBehavior>().shipNameID;
        if (!existingShipInfoThings.ContainsKey(id)) return;
        Destroy(existingShipInfoThings[id]);
        existingShipInfoThings.Remove(id);
    }
    
    public void GameOver()
    {
        AudioManager.Get().PlaySFX(gameOverSfx).volume = 0.5f;
        Time.timeScale = 0;
        gameOverScore.text = $"score: {GM.score}";
        gameOverMoney.text = $"money: {GM.money}$";
        gameOverTime.text = $"your shift lasted {timePlayed:mm} hours and {timePlayed:ss} minutes";
        gameOverPanel.SetActive(true);
    }
    
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}