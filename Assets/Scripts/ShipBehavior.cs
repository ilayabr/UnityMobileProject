using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    public ShipProperties myProperties;

    public bool isAlive = true;

    public bool isArtileryCorrect(ShipProperties.ShellTypes artileryUsed)
    {
        if (myProperties.difficulty == ShipProperties.Difficulties.JammerOnly ||
            myProperties.difficulty == ShipProperties.Difficulties.Basic)
            return true;
        if (myProperties.shellType == artileryUsed)
            return true;
        return false;
    }

    public bool IsJammerSet(float jammerValue)
    {
        if (myProperties.difficulty == ShipProperties.Difficulties.ShellOnly ||
            myProperties.difficulty == ShipProperties.Difficulties.Basic)
            return true;
        if (myProperties.jammerValue > jammerValue - 2.5f && myProperties.jammerValue < jammerValue + 2.5f)
            return true;

        return false;
    }
}