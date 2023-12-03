using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PurchasePointsPopup : BasePopup
{
    [SerializeField] TextMeshProUGUI needText;
    Uranaishi uranaishi;

    public Action onReturnFromPurchase { get; set; }


    public override void OnStart()
    {
        base.OnStart();
    }

    public void Open(Uranaishi uranaishi, int minMinutes)
    {
        this.uranaishi = uranaishi;
        base.onClickPositiveButton = OnClickPurchaseButton;
        base.onClickNegativeButton = OnClickCancelButton;
        base.Open();
        int point = uranaishi.callChargePerMin * minMinutes;
        needText.text = $"ご利用には\n最低{point}pt({minMinutes}分間の電話分)を\n所持している必要があります";
    }

    void OnClickPurchaseButton()
    {
        UIManager.i.GetModal<PurchaseModal>().Open();
        UIManager.i.GetModal<PurchaseModal>().onClose += onReturnFromPurchase;
    }

    void OnClickCancelButton()
    {

    }
}
