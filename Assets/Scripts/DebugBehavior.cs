using System.Collections;
using UnityEngine;

public class DebugBehavior : MonoBehaviour
{
    private int consecutiveTaps = 0;
    [SerializeField] private float tapResetTime = 2f;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private GameObject debugMenu;
    [SerializeField] private PauseMenu pauseMenu;
    void Update()
    {
        if (!pauseMenu.IsPaused)
        {
            debugMenu.SetActive(false);
        }
    }

    public void OnButtonPress()
    {
        if (Debug.isDebugBuild)
        {
            consecutiveTaps++;
            StartCoroutine(ResetTapCounter());
            if (consecutiveTaps < 5) return;
            debugMenu.SetActive(true);
            gameplayManager.PauseGame();
        }
    }

    private IEnumerator ResetTapCounter()
    {
        yield return new WaitForSeconds(tapResetTime);
        if (consecutiveTaps >= 0)
            consecutiveTaps--;

        yield return null;
    }
}