using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public List<ExtendedSaveData> foundSaves;

    public void OnStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnLoadGame()
    {
        foundSaves = GameManager.Get().GetAllExtSaves();
    }
}
