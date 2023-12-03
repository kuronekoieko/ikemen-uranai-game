using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchasePointElement : ObjectPoolingElement
{
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] TextMeshProUGUI omakeText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Button button;
    ChargeInfo chargeInfo;

    public override void OnInstantiate()
    {
        button.onClick.AddListener(OnClickPurchaseButton);
    }

    public void Show(ChargeInfo chargeInfo)
    {
        this.chargeInfo = chargeInfo;
        pointText.text = chargeInfo.point.ToString("N0");
        omakeText.text = $"無料ポイント{chargeInfo.omake.ToString("N0")}";
        costText.text = "￥" + chargeInfo.cost.ToString("N0");
    }

    void OnClickPurchaseButton()
    {
        SaveData.i.purchasedPoint += chargeInfo.point;
        SaveDataManager.i.Save();
    }

}
