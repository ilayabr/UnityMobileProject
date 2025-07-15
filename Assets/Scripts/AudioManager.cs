using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private AudioSource cannonShootSound;
    [SerializeField] private AudioSource backgroundMusic;

    private void Awake()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }
    
    public void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            explosionSound.Play();
        }
    }
    
    public void PlayCannonShootSound()
    {
        if (cannonShootSound != null)
        {
            cannonShootSound.Play();
        }
    }
}
