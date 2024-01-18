using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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