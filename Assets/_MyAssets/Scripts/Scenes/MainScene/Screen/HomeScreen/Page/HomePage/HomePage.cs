using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Naninovel;


public class HomePage : BasePage
{
    [SerializeField] Button conversationButton;
    [SerializeField] TodayHoroscopeButton todayHoroscopesButton;
    [SerializeField] TomorrowHoroscopeButton tomorrowHoroscopesButton;
    [SerializeField] Button selectCharacterScreenButton;
    [SerializeField] Button debugButton;


    public override void OnStart()
    {
        base.OnStart();
        todayHoroscopesButton.OnStart();
        tomorrowHoroscopesButton.OnStart();
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
        conversationButton.gameObject.SetActive(enabled);
        todayHoroscopesButton.gameObject.SetActive(enabled);
        tomorrowHoroscopesButton.gameObject.SetActive(enabled);
    }


    public override void Open()
    {
        base.Open();
        todayHoroscopesButton.OnOpen();
        tomorrowHoroscopesButton.OnOpen();
    }

    public async UniTask<bool> IsNotificationTodayHoroscope()
    {
        return await todayHoroscopesButton.IsNotification();
    }

    public async UniTask<bool> IsNotificationNextDayHoroscope()
    {
        return await tomorrowHoroscopesButton.IsNotification();
    }

    protected override void OnClose()
    {
    }
}
