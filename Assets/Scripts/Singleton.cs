using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object objLock = new();

    protected new virtual bool DontDestroyOnLoad => true;

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
        else if (instance != this)
            Destroy(gameObject);

        if (DontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    public static T Get()
    {
        lock (objLock)
        {
            if (!instance)
            {
                instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }

            return instance;
        }
    }
}