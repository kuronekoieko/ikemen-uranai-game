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
using System.Drawing.Printing;
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
        conversationButton.onClick.AddListener(OnClickCharacter);
        debugButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.Get<DebugScreen>().Open();
        });
        debugButton.gameObject.SetActive(Debug.isDebugBuild);

        this.ObserveEveryValueChanged(currentCharacterId => SaveDataManager.SaveData.currentCharacterId)
            .Subscribe(async currentCharacterId =>
            {
                await NaninovelManager.PlayHomeAsync(currentCharacterId);
                OnScriptEnd();
            })
            .AddTo(this.gameObject);
    }

    async void OnClickCharacter()
    {
        Debug.Log("キャラクリック");

        var homeText = GetHomeText();

        if (homeText == null) return;

        // Debug.Log(scriptName);
        EndScriptCommand.OnScriptEnded += (currentScriptName) =>
        {
            OnScriptEnd();
        };

        await NaninovelManager.PlayAsync("Home/" + homeText.FileName);

        todayHoroscopesButton.gameObject.SetActive(false);
        tomorrowHoroscopesButton.gameObject.SetActive(false);
        conversationButton.gameObject.SetActive(false);
    }

    void OnScriptEnd()
    {
        todayHoroscopesButton.gameObject.SetActive(true);
        tomorrowHoroscopesButton.gameObject.SetActive(true);
        conversationButton.gameObject.SetActive(true);
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

    HomeText GetHomeText()
    {

        DateTime dateTime = DateTime.Now;
        bool isHoliday = false;

        var homeTexts = CSVManager.HomeTexts
            .Where(homeText =>
            {
                if (homeText.date.priority == 0) return true;
                if (DateTime.TryParse(homeText.date.date, out DateTime dateDT))
                {
                    return dateDT == dateTime.Date;
                }
                else
                {
                    return true;
                }
            })
            .Where(homeText =>
            {
                if (homeText.day.priority == 0) return true;
                return homeText.day.IsIncludeDay(dateTime.DayOfWeek);
            })
            .Where(homeText =>
            {
                if (homeText.time.priority == 0) return true;
                return homeText.time.StartDT() <= dateTime && dateTime <= homeText.time.EndDT();
            });

        var group = homeTexts.GroupBy(homeText => homeText.Priority)
            .OrderBy(group => group.Key)
            .FirstOrDefault();
        if (group == null)
        {
            Debug.LogError("ホーム会話がみつかりません");
            return null;
        }

        foreach (var homeText in group.ToArray())
        {
            // DebugUtils.LogJson(homeText);
            // Debug.Log(homeText.date.date_name + " " + homeText.day.day_name + " " + homeText.time.time_name + " " + homeText.Priority);
        }
        //var randomHomeText = group.ToArray().GetRandom();

        return group.ToArray().GetRandom();
    }
}
