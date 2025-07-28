using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCannonShell : MonoBehaviour
{
    [SerializeField] private ShipProperties.ShellTypes myShellType = ShipProperties.ShellTypes.HE;
    [SerializeField] private ShootCanon canonScript;
    [SerializeField] private Image MyDisplay;
    [SerializeField] private Sprite ActiveShellSprite;
    [SerializeField] private Sprite InactiveShellSprite;

    private void Update()
    {
        MyDisplay.sprite = canonScript.cannonShellType == myShellType ? ActiveShellSprite : InactiveShellSprite;
    }

    public void OnButtonDown()
    {
        canonScript.cannonShellType = myShellType;
        canonScript.UpdateText();
    }
}
