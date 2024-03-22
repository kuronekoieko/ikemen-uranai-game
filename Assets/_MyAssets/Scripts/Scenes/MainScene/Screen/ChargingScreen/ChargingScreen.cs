using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    public override UniTask Close()
    {
        base.Close();
        return UniTask.DelayFrame(0);
    }
}
