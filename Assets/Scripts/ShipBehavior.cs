using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    private ShipProperties myProperties;

    float randomJammerVal;

    void Update()
    {
        Movement();
    }

    void Movement(){
        if (!gameObject.activeSelf) return;

        transform.Translate(Vector3.down * Time.deltaTime);
    }

    public bool IsArtileryCorrect(ShipProperties.ShellTypes artileryUsed)
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
        if (randomJammerVal > jammerValue - 2.5f && randomJammerVal < jammerValue + 2.5f)
            return true;

        return false;
    }

    public void SetShipProperties(ShipProperties properties){
        randomJammerVal = properties.jammerValues.GetRandom();
    }
}