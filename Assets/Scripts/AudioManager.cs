using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<string, AudioSource> activeSources = new();

    public AudioSource PlaySFX(AudioClip clip)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.playOnAwake = false;
        source.Play();

        StartCoroutine(EvilSFXKiller(source));

        return source;
    }

    IEnumerator EvilSFXKiller(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source);
    }

    public AudioSource CreateSource(string sourceName)
    {
        if (activeSources.ContainsKey(sourceName)) return null;

        activeSources[sourceName] = gameObject.AddComponent<AudioSource>();
        activeSources[sourceName].playOnAwake = false;

        return activeSources[sourceName];
    }

    public AudioSource GetSource(string sourceName)
    {
        if (activeSources.TryGetValue(sourceName, out var source))
            return source;

        return null;
    }

    public void RemoveSource(string sourceName)
    {
        if (!activeSources.ContainsKey(sourceName)) return;

        Destroy(activeSources[sourceName]);
        activeSources.Remove(sourceName);
    }
}
