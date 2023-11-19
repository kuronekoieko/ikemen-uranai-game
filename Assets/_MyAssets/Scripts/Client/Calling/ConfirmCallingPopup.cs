using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmCallingPopup : BaseCallingScreen
{
    [SerializeField] Button callButton;
    [SerializeField] Button cancelButton;
    [SerializeField] TextMeshProUGUI chargeText;
    [SerializeField] TextMeshProUGUI timeText;
    Uranaishi uranaishi1;
    public override void OnStart(CallingManager callingManager)
    {
        base.OnStart(callingManager);

        cancelButton.onClick.AddListener(() =>
        {
            callingManager.Close();
        });
        callButton.onClick.AddListener(() => OnClickCallButton());

    }

    public override void Open(Uranaishi uranaishi)
    {
        this.uranaishi = uranaishi;
        base.Open(uranaishi);

        chargeText.text = uranaishi.callChargePerMin.ToString("N0");

        int minuteInt = SaveData.i.GetAvailableDurationMin(uranaishi.callChargePerMin);
        timeText.text = minuteInt.ToString("N0");

    }

    public override void Close()
    {
        base.Close();
    }

    void OnClickCallButton()
    {
        base.manager.GetScreen<CallingScreen>().Open(uranaishi);
    }
}
