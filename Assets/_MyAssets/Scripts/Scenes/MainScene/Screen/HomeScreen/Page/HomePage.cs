using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using MainScene;
using DG.Tweening;

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
        ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today, SaveDataManager.SaveData.currentCharacterId);

        SaveDataManager.SaveData.isOpenedHoroscopeDic.TryGetValue(DateTime.Today.ToStringDate(), out bool isOpenedToday);
        if (isOpenedToday == false)
        {
            SaveDataManager.SaveData.exp += 5;
        }

        SaveDataManager.SaveData.isOpenedHoroscopeDic[DateTime.Today.ToStringDate()] = true;
        SaveDataManager.Save();
        todayHoroscopesButton.transform.DOKill(true);
    }

    void OnClickTomorrowHoroscopesButton()
    {
        var constellation = SaveDataManager.SaveData.Constellation;
        ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today.AddDays(1), SaveDataManager.SaveData.currentCharacterId);

        SaveDataManager.SaveData.isOpenedHoroscopeDic.TryGetValue(DateTime.Today.AddDays(1).ToStringDate(), out bool isOpenedNextDay);
        if (isOpenedNextDay == false)
        {
            SaveDataManager.SaveData.exp += 5;
        }

        SaveDataManager.SaveData.isOpenedHoroscopeDic[DateTime.Today.AddDays(1).ToStringDate()] = true;
        SaveDataManager.Save();
        tomorrowHoroscopesButton.transform.DOKill(true);
    }

    public override void Open()
    {
        base.Open();

        SaveDataManager.SaveData.isOpenedHoroscopeDic.TryGetValue(DateTime.Today.ToStringDate(), out bool isOpenedToday);
        SaveDataManager.SaveData.isOpenedHoroscopeDic.TryGetValue(DateTime.Today.AddDays(1).ToStringDate(), out bool isOpenedNextDay);

        if (isOpenedToday == false)
        {
            todayHoroscopesButton.transform
                .DOPunchScale(Vector3.one * 0.1f, 2f, 3, 0.1f)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        if (tomorrowHoroscopesButton.interactable && isOpenedNextDay == false)
        {
            tomorrowHoroscopesButton.transform
                .DOPunchScale(Vector3.one * 0.1f, 2f, 3, 0.1f)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

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
