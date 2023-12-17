using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HomePage : BasePage
{
    [SerializeField] Button todayHoroscopesButton;
    [SerializeField] Button tomorrowHoroscopesButton;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void Open()
    {
        base.Open();
    }

    protected override void OnClose()
    {
    }
}
