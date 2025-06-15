using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode pauseButton;
    [SerializeField] private GameObject menuContainer;

    public bool IsPaused { get; private set; }

    void Update()
    {
        if (Input.GetKeyDown(pauseButton))
        {
            TogglePause();
        }
    }

    public void TogglePause() => SetPauseState(!IsPaused);

    public void SetPauseState(bool enable)
    {
        menuContainer.SetActive(enable);
        Time.timeScale = enable ? 0 : 1;
        IsPaused = enable;
    }

    public void OnResume()
    {
        SetPauseState(false);
    }
    
    public void OnSaveGame()
    {
        GameManager.Get().SaveData();
    }
    
    public void OnLoadGame()
    {
        //lolol nothing yet
    }
    
    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
