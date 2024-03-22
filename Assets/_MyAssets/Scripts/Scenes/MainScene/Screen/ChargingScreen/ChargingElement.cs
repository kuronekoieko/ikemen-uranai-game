using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingElement : ObjectPoolingElement
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Button button;

    public override void OnInstantiate()
    {
    }

    public void OnStart()
    {
        button.AddListener(() =>
        {
            return UniTask.DelayFrame(0);
        });
    }

    public void Show(DataBase.ChargingProduct chargingProduct)
    {
        titleText.text = "有償石" + chargingProduct.JemCount + "個";
        priceText.text = "￥" + chargingProduct.jpy;
    }
}
