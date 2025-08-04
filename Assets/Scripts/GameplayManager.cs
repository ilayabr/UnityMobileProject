using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
        }
    }

    private int lives = 3;
    public int Lives
    {
        get => lives;
        set
        {
            lives = value;
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
    }


    private void Start()
    {
        GM = GameManager.Get().currentSaveData;
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
}