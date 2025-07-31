using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

public class ShootCannon : MonoBehaviour
{
    [SerializeField] private Transform scopePosition;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private TMP_Text canonText;
    [SerializeField] private AudioClip cannonShootSound;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite buttonUpImage;
    [SerializeField] private Sprite buttonPressedImage;

    public ShellTypes cannonShellType = ShellTypes.HE;
    public float cannonJammerValue = 0f;
    private bool _isLoaded = true;

    public void OnButtonPress()
    {
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        if (!_isLoaded) yield break;
        _isLoaded = false;
        RaycastHit2D hit = Physics2D.Raycast(scopePosition.position, Vector2.zero);
        if (hit.collider == null || !hit.transform.TryGetComponent(out ShipBehavior hitObject))
        {
            GameplayManager.Get().ChangeMoney(0.3f, false); // deduct money for a miss
        }
        else
        {
            if (!hitObject.OnHit(cannonShellType, cannonJammerValue))
                GameplayManager.Get().ChangeMoney(0.3f, false); // deduct money for a miss
        }

        buttonImage.sprite = buttonPressedImage;
        AudioManager.Get().PlaySFX(cannonShootSound);
        UpdateText();
        yield return new WaitForSeconds(cooldown);
        _isLoaded = true;
        UpdateText();
        buttonImage.sprite = buttonUpImage;
    }

    public void UpdateText()
    {
        if (_isLoaded)
        {
            canonText.text = "LOADED...." + cannonShellType;
            canonText.color = Color.green;
        }
        else
        {
            canonText.text = "loading.....";
            canonText.color = Color.red;
        }
    }
}