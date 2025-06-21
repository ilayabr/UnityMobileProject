using UnityEngine;
using System;

[System.Serializable]
public class SettingsSave
{
    public event Action OnSettingsChanged;

    private float volume = 50;
    public float Volume
    {
        get => volume;
        set
        {
            volume = value;
            OnSettingsChanged?.Invoke();
        }
    }

    public enum ControlSchemes
    {
        Joystick,
        Swipe
    };

    private ControlSchemes controlScheme;
    public ControlSchemes ControlScheme
    {
        get => controlScheme;
        set
        {
            controlScheme = value;
            OnSettingsChanged?.Invoke();
        }
    }

    public enum Difficulties
    {
        Easy,
        Medium,
        Hard
    };

    private Difficulties difficulty;
    public Difficulties Difficulty
    {
        get => difficulty;
        set
        {
            difficulty = value;
            OnSettingsChanged?.Invoke();
        }
    }
}
