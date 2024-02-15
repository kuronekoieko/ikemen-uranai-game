using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using MainScene;
using DG.Tweening;
using Naninovel;
using UnityEngine.EventSystems;
using SaveDataObjects;
using UniRx;
using DataBase;
using System.Linq;

public class HomePage : BasePage
{
    [SerializeField] Button conversationButton;
    [SerializeField] Button todayHoroscopesButton;
    [SerializeField] Button tomorrowHoroscopesButton;
    [SerializeField] Button selectCharacterScreenButton;
    [SerializeField] Button debugButton;
    [SerializeField] TextMeshProUGUI leftTimeText;

    readonly int openHour = 20;
    readonly int closeHour = 0;
    HoroscopeHistory horoscopeHistory;
    string Key => DateTime.Today.ToDateKey();

    public override void OnStart()
    {
        base.OnStart();
        Initialize.Instance.OnUpdate += OnUpdate;
        todayHoroscopesButton.onClick.AddListener(OnClickTodayHoroscopesButton);
        tomorrowHoroscopesButton.onClick.AddListener(OnClickTomorrowHoroscopesButton);
        selectCharacterScreenButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.Get<SelectCharacterScreen>().Open();
        });
        conversationButton.onClick.AddListener(async () =>
        {
            // 連打対策
            conversationButton.interactable = false;
            await OnClickCharacter();
            conversationButton.interactable = true;
        });
        debugButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.Get<DebugScreen>().Open();
        });
        debugButton.gameObject.SetActive(Debug.isDebugBuild);
    }

    async UniTask OnClickCharacter()
    {
        Debug.Log("キャラクリック");
        DateTime dateTime = DateTime.Now;

        var holidays = await GoogleCalendarAPI.GetHolidaysAsync(DateTime.Now.Year);

        var homeText = HomeTextManager.GetHomeText(
            SaveDataManager.SaveData.currentCharacterId,
            dateTime,
            holidays,
            CSVManager.HomeTexts);
        if (homeText == null) return;

        EnableButtons(false);
        await NaninovelManager.PlayAsync(homeText.FileName);
        EnableButtons(true);
    }

    public void EnableButtons(bool enabled)
    {
        todayHoroscopesButton.gameObject.SetActive(enabled);
        tomorrowHoroscopesButton.gameObject.SetActive(enabled);
        conversationButton.gameObject.SetActive(enabled);
    }

    void OnClickTodayHoroscopesButton()
    {
        var constellation = SaveDataManager.SaveData.Constellation;
        ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today, SaveDataManager.SaveData.GetCurrentCharacter());

        if (horoscopeHistory == null) return;

        if (horoscopeHistory.isReadTodayHoroscope == false)
        {
            SaveDataManager.SaveData.exp += 5;
        }

        SaveDataManager.SaveData.horoscopeHistories[Key].isReadTodayHoroscope = true;
        SaveDataManager.Save();
        ReturnLocalPushNotification.SetLocalPush();
        todayHoroscopesButton.transform.DOKill(true);
    }

    void OnClickTomorrowHoroscopesButton()
    {
        var constellation = SaveDataManager.SaveData.Constellation;
        ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today.AddDays(1), SaveDataManager.SaveData.GetCurrentCharacter());

        if (horoscopeHistory == null) return;

        if (horoscopeHistory.isReadNextDayHoroscope == false)
        {
            SaveDataManager.SaveData.exp += 5;
        }

        SaveDataManager.SaveData.horoscopeHistories[Key].isReadNextDayHoroscope = true;
        SaveDataManager.Save();
        ReturnLocalPushNotification.SetLocalPush();
        tomorrowHoroscopesButton.transform.DOKill(true);
    }

    public override void Open()
    {
        base.Open();

        SaveDataManager.SaveData.horoscopeHistories.TryGetValue(Key, out horoscopeHistory);

        if (horoscopeHistory == null) return;

        if (horoscopeHistory.isReadTodayHoroscope == false)
        {
            todayHoroscopesButton.transform
                .DOPunchScale(Vector3.one * 0.1f, 2f, 3, 0.1f)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .OnKill(() =>
                {
                    todayHoroscopesButton.transform.localScale = Vector3.one;
                });
        }

        if (tomorrowHoroscopesButton.interactable && horoscopeHistory.isReadNextDayHoroscope == false)
        {
            tomorrowHoroscopesButton.transform
                .DOPunchScale(Vector3.one * 0.1f, 2f, 3, 0.1f)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .OnKill(() =>
                {
                    todayHoroscopesButton.transform.localScale = Vector3.one;
                });
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
