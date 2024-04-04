using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HomeFooter : MonoBehaviour
{
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

