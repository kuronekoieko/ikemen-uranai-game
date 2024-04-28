using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
//using Naninovel;


public class HomePage : BasePage
{
    [SerializeField] Button conversationButton;
    [SerializeField] TodayHoroscopeButton todayHoroscopesButton;
    [SerializeField] TomorrowHoroscopeButton tomorrowHoroscopesButton;
    [SerializeField] Button selectCharacterScreenButton;
    [SerializeField] SideBanner sideBanner;

    public override void OnStart()
    {
        base.OnStart();
        todayHoroscopesButton.OnStart();
        tomorrowHoroscopesButton.OnStart();
        sideBanner.OnStart();
        selectCharacterScreenButton.AddListener(() =>
        {
            ScreenManager.Instance.Get<SelectCharacterScreen>().Open();
            return UniTask.DelayFrame(0);
        });
        conversationButton.AddListener(OnClickCharacter);
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
        conversationButton.gameObject.SetActive(enabled);
        todayHoroscopesButton.gameObject.SetActive(enabled);
        tomorrowHoroscopesButton.gameObject.SetActive(enabled);
    }


    public override void Open()
    {
        base.Open();
        todayHoroscopesButton.OnOpen();
        tomorrowHoroscopesButton.OnOpen();
        sideBanner.OnOpen();
    }

    public bool IsNotificationTodayHoroscope()
    {
        return todayHoroscopesButton.IsNotification();
    }

    public bool IsNotificationNextDayHoroscope()
    {
        return tomorrowHoroscopesButton.IsNotification();
    }

    protected override void OnClose()
    {
    }
}
