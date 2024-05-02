using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using TMPro;
using SaveDataObjects;
using Cysharp.Threading.Tasks;
using UniRx;

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

        this.ObserveEveryValueChanged(isRead => IsNotification())
        .Subscribe(isRead =>
        {
            if (isRead)
            {
                base.Anim();
            }
            else
            {
                Kill();
            }
        })
        .AddTo(gameObject);
    }

    public override void OnOpen()
    {

    }

    public bool IsNotification()
    {
        HoroscopeHistory horoscopeHistory = GetHoroscopeHistory();

        if (horoscopeHistory == null) return false;
        return horoscopeHistory.isReadNextDayHoroscope == false && button.interactable;
    }

    protected async override UniTask OnClick()
    {
        HoroscopeHistory horoscopeHistory = GetHoroscopeHistory();
        if (horoscopeHistory == null) return;

        if (horoscopeHistory.isReadNextDayHoroscope == false)
        {
            // SaveDataManager.SaveData.exp += 5;
        }

        SaveDataManager.SaveData.horoscopeHistories[Key].isReadNextDayHoroscope = true;
        await SaveDataManager.SaveAsync();
        LocalPushNotificationManager.SetLocalPush();


        var constellation = SaveDataManager.SaveData.Constellation;
        await ScreenManager.Instance.Get<HoroscopeScreen>().Open(
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
            leftTimeText.text = timeSpan.ToString(@"h\:m");
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
