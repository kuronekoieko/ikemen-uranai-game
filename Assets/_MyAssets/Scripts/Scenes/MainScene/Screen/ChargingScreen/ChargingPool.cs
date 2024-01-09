using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingPool : ObjectPooling<ChargingElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void Show(DataBase.Character[] characters)
    {
        foreach (var item in characters)
        {
            var instance = GetInstance();
            instance.Show();
        }
    }
}
