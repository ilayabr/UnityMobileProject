using System;
using TMPro;
using UnityEngine;

public class ShipInfoCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Setup(ShipBehavior ship)
    {
        text.text = $"Obj-{ship.shipNameID}:------";
        if (ship.MyProperties.difficulty == ShipProperties.Difficulties.ShellAndJammer || ship.MyProperties.difficulty == ShipProperties.Difficulties.ShellOnly)
            text.text += $"\n Ammo-{Enum.GetName(typeof(ShellTypes), ship.shellType)}........";
        if (ship.MyProperties.difficulty == ShipProperties.Difficulties.ShellAndJammer || ship.MyProperties.difficulty == ShipProperties.Difficulties.JammerOnly)
        text.text += $"\n Jammer:{ship.RandomJammerVal}.....\n JRange:~{ship.RandomJammerOffset:.2}";
    }
}
