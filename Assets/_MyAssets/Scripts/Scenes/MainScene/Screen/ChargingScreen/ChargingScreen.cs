using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargingScreen : BaseScreen
{

    [SerializeField] Button closeButton;
    [SerializeField] ChargingPool chargingPool;

    public override void OnStart()
    {
        base.OnStart();
        closeButton.onClick.AddListener(Close);
        chargingPool.OnStart();
        chargingPool.Show(CSVManager.Instance.ChargingProducts);
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
