using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DoneReservePopup : BasePopup
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI startDateTimeText;
    [SerializeField] TextMeshProUGUI durationText;


    public override void OnStart()
    {
        base.OnStart();
    }

    public void Open(Uranaishi uranaishi, DateTime startDateTime, int durationMin)
    {
        base.onClickPositiveButton = () =>
        {
            UIManager.i.CloseAllModals(4);
        };
        base.Open();

        nameText.text = uranaishi.name;
        startDateTimeText.text = startDateTime.ToString("MM/dd(ddd) HH:mm");
        durationText.text = durationMin + "分";
    }
}
