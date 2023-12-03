using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : SingletonMonoBehaviour<LoadingScreen>
{
    public void OnStart()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
