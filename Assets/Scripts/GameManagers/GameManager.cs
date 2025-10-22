using UnityEngine;

public abstract class GameManager<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
    }
}
