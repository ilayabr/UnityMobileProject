using UnityEngine;

[CreateAssetMenu(fileName = "ShipProperties", menuName = "Scriptable Objects/ShipProperties")]
public class ShipProperties : ScriptableObject
{
    public string shipName;
    public Range speed;
    public int scoreWorth = 1;
    public float damage = 10f;
    public float value = 100f;
    public Range jammerValues = new Range(0f, 60f);
    public Range jammerOffset = new Range(1f, 3f);
    public AudioClip exploadSound;

    public Sprite sprite;

    public enum Difficulties
    {
        Basic,
        ShellOnly,
        JammerOnly,
        ShellAndJammer
    }

    public Difficulties difficulty;
}