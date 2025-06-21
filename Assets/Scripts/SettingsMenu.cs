using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_Dropdown scheme;
    [SerializeField] private TMP_Dropdown difficulty;
    [SerializeField] private AudioMixer mixer;

    void Awake()
    {
        GameManager.Get().OnSettingsLoaded += SettingsLoaded;
        GameManager.Get().OnSavedSettingsChanged += SettingsChanged;
    }

    void OnEnable()
    {
        GameManager.Get().LoadSettings();
    }

    void OnDisable()
    {
        GameManager.Get().SaveSettings();
    }

    void SettingsLoaded()
    {
        var settings = GameManager.Get().CurrentSettingsData;

        musicSlider.value = settings.Volume;

        scheme.ClearOptions();
        scheme.AddOptions(Enum.GetNames(typeof(SettingsSave.ControlSchemes)).ToList());
        scheme.value = (int)settings.ControlScheme;

        difficulty.ClearOptions();
        difficulty.AddOptions(Enum.GetNames(typeof(SettingsSave.Difficulties)).ToList());
        difficulty.value = (int)settings.Difficulty;

        SettingsChanged();
    }

    public void OnVolumeChanged(float volume)
    {
        GameManager.Get().CurrentSettingsData.Volume = volume;
    }

    public void OnSchemeChanged(int scheme)
    {
        GameManager.Get().CurrentSettingsData.ControlScheme = (SettingsSave.ControlSchemes)scheme;
    }

    public void OnDiffChanged(int diff)
    {
        GameManager.Get().CurrentSettingsData.Difficulty = (SettingsSave.Difficulties)diff;
    }

    void SettingsChanged()
    {
        mixer.SetFloat("volume", GameManager.Get().CurrentSettingsData.Volume - 80);
    }
}
