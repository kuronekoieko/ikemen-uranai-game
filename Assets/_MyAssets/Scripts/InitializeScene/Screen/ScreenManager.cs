using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScreenManager : MonoBehaviour
{
    BaseScreen[] baseScreens;
    public static ScreenManager Instance;

    public void OnStart()
    {
        Instance = this;
        StartScreens();
    }

    void StartScreens()
    {
        baseScreens = GetComponentsInChildren<BaseScreen>(true);
        foreach (var baseModal in baseScreens)
        {
            baseModal.OnStart();
        }
    }

    public T Get<T>() where T : BaseScreen
    {
        T subClass = baseScreens.Select(_ => _ as T).FirstOrDefault(_ => _);
        return subClass;
    }
}