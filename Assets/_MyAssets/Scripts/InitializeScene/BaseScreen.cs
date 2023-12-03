using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScreen<T> : MonoBehaviour where T : Component
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>(true);
            }
            return instance;
        }
    }
    private static T instance;


    public abstract void OnStart();
    public abstract void Open();

}
