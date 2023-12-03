using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmHangUpPopup : BaseCallingScreen
{
    [SerializeField] Button hangupButton;
    [SerializeField] Button cancelButton;

    public override void OnStart(CallingManager callingManager)
    {
        base.OnStart(callingManager);

        cancelButton.onClick.AddListener(() =>
        {
            Close();
        });
        hangupButton.onClick.AddListener(() =>
        {
            HangUp();
        });
    }

    public override void Open(Uranaishi uranaishi)
    {
        base.Open(uranaishi);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    void HangUp()
    {
        base.manager.GetScreen<CallingScreen>().StopTimer();
        base.manager.GetScreen<InputReviewScreen>().Open(uranaishi);
    }
}
