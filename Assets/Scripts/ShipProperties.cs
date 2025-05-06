using UnityEngine;

[CreateAssetMenu(fileName = "ShipProperties", menuName = "Scriptable Objects/ShipProperties")]
public class ShipProperties : ScriptableObject
{
    public Range speed;
    public float damage = 10f;
    public float value = 100f;

    public enum Difficulties{
        Basic,
        ShellOnly,
        JammerOnly,
        ShellAndJammer
    }

    public Difficulties difficulty;
}