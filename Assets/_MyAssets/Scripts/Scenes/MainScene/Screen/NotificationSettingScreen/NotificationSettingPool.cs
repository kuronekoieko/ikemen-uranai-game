using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationSettingPool : ObjectPooling<NotificationSettingElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void Show(NotificationSettingObj[] list)
    {
        foreach (var item in list)
        {
            var instance = GetInstance();
            instance.Show(item);
        }
    }
}
