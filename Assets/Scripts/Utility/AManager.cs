using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SingletonDestroyMode { GameObject, Script, Nothing, Custom }

/// <summary>Helper class that defines an abstract singleton inheriting from MonoBehaviour </summary>
/// <typeparam name="T">The inheriting class.</typeparam>
public abstract class AManager<T> : MonoBehaviour where T : class
{
    public static T Instance { get; protected set; }
    
    [SerializeField]
    private SingletonDestroyMode _destroyMode;

    [SerializeField]
    private bool _dontDestroyOnLoad;

    protected abstract void OnAwake();

    private void Awake()
    {
        if (Instance != null)
        {
            if (_destroyMode == SingletonDestroyMode.Custom)
                OnCustomDestroyMode(Instance, this as T);

            else if (_destroyMode == SingletonDestroyMode.GameObject)
                Destroy(gameObject);

            else if (_destroyMode == SingletonDestroyMode.Script)
                Destroy(this);

            else
            {   
                //Debug.Log("Multiple instances of " + gameObject.name + " detected. Intended?!");
            }
        }

        else
        {
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            Instance = this as T;
            OnAwake();
        }
    }

    protected virtual void OnCustomDestroyMode(T oldInstance, T newInstance)
    {

    }
}
