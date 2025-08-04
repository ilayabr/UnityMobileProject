using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeCannonShell : MonoBehaviour
{
    [SerializeField] private ShellTypes myShellType = ShellTypes.HE;
    [SerializeField] private ShootCannon cannonScript;
    [SerializeField] private Image MyDisplay;
    [SerializeField] private Sprite ActiveShellSprite;
    [SerializeField] private Sprite InactiveShellSprite;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip changeShellSound;

    private void Update()
    {
        MyDisplay.sprite = cannonScript.cannonShellType == myShellType ? ActiveShellSprite : InactiveShellSprite;
    }

    public void OnButtonDown()
    {
        AudioManager.Get().PlaySFX(buttonSound);
        cannonScript.cannonShellType = myShellType;
        AudioManager.Get().PlaySFX(changeShellSound).volume = 0.2f;
        StartCoroutine(cannonScript.Reload());
    }
}
