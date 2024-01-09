using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingPool : ObjectPooling<ChargingElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void Show(DataBase.ChargingProduct[] chargingProducts)
    {
        foreach (var item in chargingProducts)
        {
            var instance = GetInstance();
            instance.Show(item);
        }
    }
}
