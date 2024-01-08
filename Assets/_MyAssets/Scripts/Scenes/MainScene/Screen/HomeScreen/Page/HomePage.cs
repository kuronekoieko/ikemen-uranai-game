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

public class HomePage : BasePage
{
    [SerializeField] Button conversationButton;
    [SerializeField] Button todayHoroscopesButton;
    [SerializeField] Button tomorrowHoroscopesButton;
    [SerializeField] Button selectCharacterScreenButton;
    [SerializeField] TextMeshProUGUI leftTimeText;

    readonly int openHour = 20;
    readonly int closeHour = 0;


    public override async void OnStart()
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


        string scriptName = "HomeIdle";
        // https://naninovel.com/ja/guide/integration-options#%E6%89%8B%E5%8B%95%E5%88%9D%E6%9C%9F%E5%8C%96
        await RuntimeInitializer.InitializeAsync();



        var player = Engine.GetService<IScriptPlayer>();
        // ツールバー Naninovel -> Resources -> Scripts でスクリプト割当
        await player.PreloadAndPlayAsync(scriptName);

        Debug.Log("Naninovel読み込み終了");


    }

    async void OnClickCharacter()
    {
        Debug.Log("キャラクリック");
        string[] scriptNames = new string[]
        {
            "HomeTest",
            "HomeTest 1",
            "HomeTest 2",
        };

        EndScriptCommand.OnScriptEnded += (currentScriptName) =>
        {
            todayHoroscopesButton.gameObject.SetActive(true);
            tomorrowHoroscopesButton.gameObject.SetActive(true);
            conversationButton.gameObject.SetActive(true);
        };

        var player = Engine.GetService<IScriptPlayer>();
        // ツールバー Naninovel -> Resources -> Scripts でスクリプト割当
        await player.PreloadAndPlayAsync(scriptNames.GetRandom());

        todayHoroscopesButton.gameObject.SetActive(false);
        tomorrowHoroscopesButton.gameObject.SetActive(false);
        conversationButton.gameObject.SetActive(false);
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
