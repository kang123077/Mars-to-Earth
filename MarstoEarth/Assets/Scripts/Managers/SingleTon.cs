using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.Log($"Singleton<{typeof(T)}> instance is missing in the scene.");
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log($"Destroying duplicate Singleton<{typeof(T)}>. Only one instance should exist.");
            Destroy(gameObject);
            return;
        }

        instance = this as T;
    }
}