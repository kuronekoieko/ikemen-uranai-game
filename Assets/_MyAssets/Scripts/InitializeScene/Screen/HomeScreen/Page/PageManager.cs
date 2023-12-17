using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PageManager : MonoBehaviour
{
    BasePage[] basePages;
    public static PageManager Instance;

    public void OnStart()
    {
        Instance = this;
        StartScreens();
    }

    void StartScreens()
    {
        basePages = GetComponentsInChildren<BasePage>(true);
        foreach (var baseModal in basePages)
        {
            baseModal.OnStart();
        }
    }

    public T Get<T>() where T : BasePage
    {
        T subClass = basePages.Select(_ => _ as T).FirstOrDefault(_ => _);
        return subClass;
    }

    public BasePage GetActive()
    {
        BasePage basePage = basePages
            .FirstOrDefault(_ => _.gameObject.activeSelf);

        return basePage;
    }
}
