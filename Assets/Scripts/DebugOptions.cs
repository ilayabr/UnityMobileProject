using System;
using UnityEngine;

public class DebugOptions : MonoBehaviour
{
    private enum ButtonType
    {
        Score,
        Money,
        Lives,
        TimePlayed,
        ResetGame,
    }
    
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private ButtonType thisButtonType;

    public void OnButtonClick()
    {
        switch (thisButtonType)
        {
            case ButtonType.Score:
                gameplayManager.ChangeScore(1000);
                break;
            case ButtonType.Money:
                gameplayManager.ChangeMoney(1000, true);
                break;
            case ButtonType.Lives:
                gameplayManager.Lives++; //lives not yet implemented!
                break;
            case ButtonType.TimePlayed:
                gameplayManager.TimePlayed += TimeSpan.FromMinutes(10);
                break;
            case ButtonType.ResetGame:
                gameplayManager.ChangeScore(-gameplayManager.Score);
                gameplayManager.ChangeMoney(gameplayManager.Money, false);
                gameplayManager.Lives = 3;
                gameplayManager.TimePlayed = TimeSpan.Zero;
                break;
            default:
                //Debug.Log("Unhandled button type: " + thisButtonType);
                break;
        }
    }
}
