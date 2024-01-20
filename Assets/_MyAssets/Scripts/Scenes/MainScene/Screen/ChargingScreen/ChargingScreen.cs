using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargingScreen : BaseScreen
{

    [SerializeField] ChargingPool chargingPool;

    public override void OnStart()
    {
        base.OnStart();
        chargingPool.OnStart();
        chargingPool.Show(CSVManager.ChargingProducts);
    }

    public override void Open()
    {
        base.Open();

    }

    public override void Close()
    {
        base.Close();
    }
}
