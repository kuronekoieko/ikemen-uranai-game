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
    [SerializeField] Button debugButton;
    [SerializeField] SideBanner sideBanner;
    [SerializeField] Button popupButton;



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
        debugButton.AddListener(() =>
        {
            ScreenManager.Instance.Get<DebugScreen>().Open();
            return UniTask.DelayFrame(0);
        });
        debugButton.gameObject.SetActive(Debug.isDebugBuild);
        popupButton.AddListener(async () =>
        {
            await PopupManager.Instance.GetCommonPopup().ShowAsync(
"",
"メンテナンス中です\nしばらく時間をおいてお試しください。",
"OK",
"キャンセル"
);

        });
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
