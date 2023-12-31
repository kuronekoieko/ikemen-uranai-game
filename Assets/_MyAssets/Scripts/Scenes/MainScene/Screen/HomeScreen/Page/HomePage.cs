using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using MainScene;

public class HomePage : BasePage
{
    [SerializeField] Button todayHoroscopesButton;
    [SerializeField] Button tomorrowHoroscopesButton;
    [SerializeField] TextMeshProUGUI leftTimeText;

    readonly int openHour = 20;
    readonly int closeHour = 0;


    public override void OnStart()
    {
        base.OnStart();
        Initialize.Instance.OnUpdate += OnUpdate;
        todayHoroscopesButton.onClick.AddListener(OnClickTodayHoroscopesButton);
        tomorrowHoroscopesButton.onClick.AddListener(OnClickTomorrowHoroscopesButton);
    }

    void OnClickTodayHoroscopesButton()
    {
        var constellation = SaveDataManager.SaveData.Constellation;
        ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation);
    }

    void OnClickTomorrowHoroscopesButton()
    {
        var constellation = SaveDataManager.SaveData.Constellation;
        ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation);
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

        Show();
    }


    void Show()
    {
        DateTime now = DateTime.Now;
        DateTime today = DateTime.Today;
        bool isOpenTomorrowHoroscope = IsOpenTomorrowHoroscope(now, openHour, closeHour);
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

    public static bool IsOpenTomorrowHoroscope(DateTime now, int openHour, int closeHour)
    {
        bool isOpenTomorrowHoroscope = openHour <= now.Hour || now.Hour < closeHour;
        return isOpenTomorrowHoroscope;
    }
}
