using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootCanon : MonoBehaviour
{
    [SerializeField] Transform scopePosition;
    [SerializeField] Camera gameCamera;
    [SerializeField] float cooldown = 5f;
    [SerializeField] TMP_Text canonText;
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
        if (hit.collider == null || !hit.transform.TryGetComponent(out IHitable hitObject))
        {
            GameplayManager.Get().ChangeMoney(0.3f, false); // deduct money for a miss
        }
        else
        {
            hitObject.OnHit();
        }

        canonText.text = "loading.....";
        yield return new WaitForSeconds(cooldown);
        _isLoaded = true;
        canonText.text = "LOADED....HE"; //will display shell type later
    }
}