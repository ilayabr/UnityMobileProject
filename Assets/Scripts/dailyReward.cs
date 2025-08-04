using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class DailyRewardState
{
    public int streak = 0;
    public DateTime lastStreakBroken = DateTime.Now;
    public int lastRewardClaimed;
}

public class dailyReward : MonoBehaviour
{
    public TextMeshProUGUI streakText;
    public GameObject container;

    DailyRewardState state;

    void Awake()
    {
        DailyRewardManager.OnStreakChecked += OnStreakCheck;
    }

    public void OnStreakCheck(bool isStreakClaimable, DailyRewardState state)
    {
        if (!isStreakClaimable) return;

        container.SetActive(true);

        streakText.text = $"Streak {state.streak + 1}/7";

        this.state = state;
    }

    public void ClaimReward()
    {
        state.streak = Mathf.Min(state.streak + 1, 7);

        state.lastRewardClaimed = state.streak;

        if (state.streak == 7)
        {
            state.streak = 0;
            state.lastRewardClaimed = 0;
            state.lastStreakBroken = DateTime.Now;
        }

        container.SetActive(false);

        DailyRewardManager.Save(state);
    }
}
