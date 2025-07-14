using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class DailyRewardManager : Singleton<DailyRewardManager>
{
    protected override bool DontDestroyOnLoad => false;
    private const string SaveKey = "daily_reward_state";
    private DailyRewardState state;

    public static event UnityAction<bool, DailyRewardState> OnStreakChecked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = Load();
        var timeSenseLastOpen = DateTime.Now - state.lastStreakBroken;

        bool didBreakStreak = timeSenseLastOpen.Days > state.streak + 2;
        bool didContinueStreak = timeSenseLastOpen.Days == state.streak + 1;

        if (didBreakStreak)
        {
            state.streak = 0;
            state.lastRewardClaimed = 0;
            state.lastStreakBroken = DateTime.Now;
        }

        OnStreakChecked?.Invoke(didContinueStreak && !didBreakStreak && state.lastRewardClaimed == state.streak, state);

        Save(state);
    }

    public static DailyRewardState Load()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return new();

        string json = PlayerPrefs.GetString(SaveKey);
        return JsonConvert.DeserializeObject<DailyRewardState>(json);
    }

    public static void Save(DailyRewardState state)
    {
        string json = JsonConvert.SerializeObject(state);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }
}
