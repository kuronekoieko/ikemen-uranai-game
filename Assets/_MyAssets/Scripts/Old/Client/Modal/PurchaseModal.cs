using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseModal : BaseModal
{
    [SerializeField] PurchasePointManager purchasePointManager;
    [SerializeField] Text sumPointText;
    [SerializeField] Text pointsText;

    public override void OnStart()
    {
        base.OnStart();
        purchasePointManager.OnStart();
        purchasePointManager.ShowElements(Constant.Instance.chargeInfos.value);
    }


    void ShowTexts()
    {
        pointsText.text = $"購入分 {SaveData.i.purchasedPoint.ToString("N0")}    " + $"無料ポイント {SaveData.i.freePoint.ToString("N0")}";
        sumPointText.text = (SaveData.i.purchasedPoint + SaveData.i.freePoint).ToString("N0");
    }

    private void Update()
    {
        ShowTexts();
    }

    public void Open()
    {
        base.OpenAnim();
    }
}
