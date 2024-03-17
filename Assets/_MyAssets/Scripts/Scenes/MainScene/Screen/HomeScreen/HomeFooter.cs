using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using MainScene;
using UniRx;

public class HomeFooter : MonoBehaviour
{
    [SerializeField] FooterToggleData[] footerToggleDatas;

    public void OnStart()
    {
        var footerToggleControllers = GetComponentsInChildren<BaseFooterToggleController>(true);
        foreach (var footerToggleController in footerToggleControllers)
        {
            footerToggleController.OnStart();
        }
        footerToggleControllers[0].ToggleOn(true);
    }
}

