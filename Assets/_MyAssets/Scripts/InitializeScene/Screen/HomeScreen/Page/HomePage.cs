using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HomePage : BasePage
{
    [SerializeField] Button todayHoroscopesButton;
    [SerializeField] Button tomorrowHoroscopesButton;
    [SerializeField] TextMeshProUGUI leftTimeText;

    readonly int closeHour = 5;
    readonly int openHour = 21;


    public override void OnStart()
    {
        base.OnStart();
        Initialize.Instance.OnUpdate += OnUpdate;
    }

    public override void Open()
    {
        base.Open();
    }

    protected override void OnClose()
    {
    }

    void OnUpdate()
    {
        DateTime now = DateTime.Now;
        DateTime today = DateTime.Today;
        Show(now, today);
    }


    void Show(DateTime now, DateTime today)
    {
        bool isOpenTomorrowHoroscope = openHour < now.Hour || now.Hour < closeHour;
        if (isOpenTomorrowHoroscope == false)
        {
            var tomorrowHoroscopeDT = today.AddHours(openHour);
            // 表示が秒数切捨てなので、１分足す
            TimeSpan timeSpan = tomorrowHoroscopeDT - now + new TimeSpan(0, 1, 0);
            leftTimeText.text = "占いまで\nあと" + timeSpan.ToString(@"h\時\間m\分");
        }

        leftTimeText.gameObject.SetActive(!isOpenTomorrowHoroscope);
        tomorrowHoroscopesButton.interactable = isOpenTomorrowHoroscope;
    }
}
