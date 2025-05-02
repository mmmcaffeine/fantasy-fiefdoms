using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    // ReSharper disable StaticMemberInGenericType

    // In this scenario we explicitly want different singletons to have different lock objects and lifetime flags
    private static readonly object _instanceLock = new();
    private static bool _quitting;

    // ReSharper restore StaticMemberInGenericType

    public static T Instance
    {
        get
        {
            // TODO Double-check singleton implementation and decide whether we should be double-locking
            lock (_instanceLock)
            {
                if (_instance is null && !_quitting)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance is null)
                    {
                        var gameObject = new GameObject(typeof(T).ToString());

                        _instance = gameObject.AddComponent<T>();
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance is null)
        {
            _instance = gameObject.GetComponent<T>();
        }
        else if (_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            throw new Exception($"Instance of {typeof(T).FullName} already exists. Removing {ToString()}");
        }

        Init();
    }

    protected virtual void OnApplicationQuit()
    {
        _quitting = true;
    }

    protected virtual void Init()
    {
    }
}
