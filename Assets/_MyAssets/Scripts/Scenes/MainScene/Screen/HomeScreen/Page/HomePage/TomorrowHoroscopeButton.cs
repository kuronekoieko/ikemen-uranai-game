using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using MainScene;
using TMPro;
using SaveDataObjects;
using Cysharp.Threading.Tasks;

public class TomorrowHoroscopeButton : BaseHoroscopeButton
{

    [SerializeField] TextMeshProUGUI leftTimeText;
    [SerializeField] Image leftTimeBGImage;
    readonly int openHour = 20;
    readonly int closeHour = 0;

    public override void OnStart()
    {
        base.OnStart();

        Initialize.Instance.OnUpdate += OnUpdate;
    }

    public override async void OnOpen()
    {
        bool isNotification = await IsNotification();
        if (!isNotification) return;
        base.Anim();
    }

    public async UniTask<bool> IsNotification()
    {
        HoroscopeHistory horoscopeHistory = await GetHoroscopeHistory();

        if (horoscopeHistory == null) return false;
        return horoscopeHistory.isReadNextDayHoroscope == false && button.interactable;
    }

    protected async override void OnClick()
    {
        HoroscopeHistory horoscopeHistory = await GetHoroscopeHistory();
        if (horoscopeHistory == null) return;

        if (horoscopeHistory.isReadNextDayHoroscope == false)
        {
            SaveDataManager.SaveData.exp += 5;
        }

        SaveDataManager.SaveData.horoscopeHistories[Key].isReadNextDayHoroscope = true;
        await SaveDataManager.SaveAsync();
        ReturnLocalPushNotification.SetLocalPush();
        transform.DOKill(true);

        var constellation = SaveDataManager.SaveData.Constellation;
        ScreenManager.Instance.Get<HoroscopeScreen>().Open(
            constellation,
            DateTime.Today.AddDays(1),
            SaveDataManager.SaveData.GetCurrentCharacter());
    }

    void OnUpdate()
    {
        Show();
    }


    void Show()
    {
        DateTime now = DateTime.Now;
        DateTime today = DateTime.Today;
        bool isUnlock = IsUnlock(now, openHour, closeHour);
        if (isUnlock == false)
        {
            var tomorrowHoroscopeDT = today.AddHours(openHour);
            // 表示が秒数切捨てなので、１分足す
            TimeSpan timeSpan = tomorrowHoroscopeDT - now + new TimeSpan(0, 1, 0);
            leftTimeText.text = "占いまで\nあと" + timeSpan.ToString(@"h\時\間m\分");
        }

        leftTimeText.gameObject.SetActive(!isUnlock);
        leftTimeBGImage.gameObject.SetActive(!isUnlock);
        button.interactable = isUnlock;
    }

    public static bool IsUnlock(DateTime now, int openHour, int closeHour)
    {
        bool isOpenTomorrowHoroscope = openHour <= now.Hour || now.Hour < closeHour;
        return isOpenTomorrowHoroscope;
    }

}
