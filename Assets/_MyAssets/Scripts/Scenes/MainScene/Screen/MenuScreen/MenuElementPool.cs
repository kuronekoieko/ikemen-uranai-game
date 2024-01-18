using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuElementPool : ObjectPooling<MenuElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void Show(MenuElementObj[] menuElementObjs)
    {
        foreach (var item in menuElementObjs)
        {
            var instance = GetInstance();
            instance.Show(item);
        }
    }
}


public class MenuElementObj
{
    public string title;
    public UnityAction onClick;
}