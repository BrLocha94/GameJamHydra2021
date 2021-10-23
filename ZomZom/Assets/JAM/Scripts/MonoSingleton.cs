using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
        private set { }
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this as T;
        DontDestroyOnLoad(gameObject);

        ExecuteOnAwake();
    }

    protected virtual void ExecuteOnAwake() { }
}
