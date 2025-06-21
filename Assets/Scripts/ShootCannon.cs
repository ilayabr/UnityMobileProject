using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootCanon : MonoBehaviour
{
    [SerializeField] Transform scopePosition;
    [SerializeField] Camera gameCamera;
    [SerializeField] float cooldown = 5f;
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
            Debug.Log("Whiffed!");
        }
        else
        {
            Debug.Log("Hit!");
            hitObject.OnHit();
        }

        Debug.Log("Reloading...");
        yield return new WaitForSeconds(cooldown);
        _isLoaded = true;
        Debug.Log("Reloaded!");
    }
}