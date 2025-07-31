using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeCannonShell : MonoBehaviour
{
    [SerializeField] private ShellTypes myShellType = ShellTypes.HE;
    [FormerlySerializedAs("canonScript")] [SerializeField] private ShootCannon cannonScript;
    [SerializeField] private Image MyDisplay;
    [SerializeField] private Sprite ActiveShellSprite;
    [SerializeField] private Sprite InactiveShellSprite;

    private void Update()
    {
        MyDisplay.sprite = cannonScript.cannonShellType == myShellType ? ActiveShellSprite : InactiveShellSprite;
    }

    public void OnButtonDown()
    {
        cannonScript.cannonShellType = myShellType;
        cannonScript.UpdateText();
    }
}
