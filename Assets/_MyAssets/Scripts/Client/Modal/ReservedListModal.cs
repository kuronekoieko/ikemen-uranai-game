using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class ReservedListModal : BaseModal
{
    [SerializeField] ReservedInfoManager reservedInfoManager;
    [SerializeField] Button rulesButton;

    public override void OnStart()
    {
        base.OnStart();
        reservedInfoManager.OnStart();
        rulesButton.onClick.AddListener(OnClickRulesButton);

    }

    public void Open()
    {
        base.OpenAnim();

        Reserve[] reserves = SaveData.i.reserves
            .Where(r => r.startSDT.dateTime != null)
            .Where(r => r.startSDT.dateTime.Value.AddMinutes(r.durationMin) > DateTime.Now)
            .OrderByDescending(r => r.startSDT.dateTime)
            .ToArray();

        reservedInfoManager.ShowElement(reserves);
    }

    void OnClickRulesButton()
    {
        ChromeCustomTabs.OpenURL("");
    }
}
