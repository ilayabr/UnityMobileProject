using UnityEngine;

[CreateAssetMenu(fileName = "ShipProperties", menuName = "Scriptable Objects/ShipProperties")]
public class ShipProperties : ScriptableObject
{
    public Range speed;
    public float damage = 10f;
    public float value = 100f;
    private static Range _jammerValues = new Range(30f, 60f);

    public enum Difficulties{
        Basic,
        ShellOnly,
        JammerOnly,
        ShellAndJammer
    }
    public Difficulties difficulty;

    public enum ShellTypes
    {
        HE,
        AP,
        HEAT
    }
    public ShellTypes shellType;

    public float jammerValue = _jammerValues.GetRandom();
}