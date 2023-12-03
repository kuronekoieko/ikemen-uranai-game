using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ReservedInfoElement : ObjectPoolingElement
{

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Image iconImage;
    [SerializeField] Button cancelReserveButton;

    public override void OnInstantiate()
    {
        cancelReserveButton.onClick.AddListener(OnClickCancelReserveButton);
    }


    public async void Show(Reserve reserve)
    {
        nameText.text = "";
        dateText.text = "";
        timeText.text = "予約時間 : ";
        Uranaishi uranaishi = await FirebaseDatabaseManager.i.GetUserData(reserve.uranaishiId);

        iconImage.sprite = null;
        uranaishi.GetIcon((sprite) =>
        {
            iconImage.sprite = sprite;
        });


        nameText.text = uranaishi.name;
        DateTime? startDT = reserve.startSDT.dateTime;
        DateTime? endDT = startDT.Value.AddMinutes(reserve.durationMin);

        dateText.text = startDT.ToStringIncludeEmpty("MM/dd(ddd)");

        timeText.text = "予約時間 : "
            + startDT.ToStringIncludeEmpty("HH:mm")
            + "～"
            + endDT.ToStringIncludeEmpty("HH:mm");
    }

    void OnClickCancelReserveButton()
    {

    }
}
