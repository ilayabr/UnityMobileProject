using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public List<ExtendedSaveData> foundSaves;

    public async void OnStartGame()
    {
        DataAnalyticsManager.Get().TrackData("game_start");

        await GameManager.Get().TransitionToScene("GameScene", .5f);
    }

    public void OnLoadGame()
    {
        foundSaves = GameManager.Get().GetAllExtSaves();
    }
}
