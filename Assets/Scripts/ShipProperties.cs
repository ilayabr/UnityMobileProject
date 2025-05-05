using UnityEngine;

[CreateAssetMenu(fileName = "ShipProperties", menuName = "Scriptable Objects/ShipProperties")]
public class ShipProperties : ScriptableObject
{
    public float speed = 0.5f;
    public float damage = 10f;
    public float value = 100f;
    
    public enum AttackType
    {
        Normal,
        HE,
        AP
    }
    
    public AttackType attackType = AttackType.Normal;
}
