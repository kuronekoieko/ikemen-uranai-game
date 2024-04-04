using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SaveDataObjects;
using Cysharp.Threading.Tasks;
using UniRx;


public class TodayHoroscopeButton : BaseHoroscopeButton
{

    public override void OnStart()
    {
        base.OnStart();

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
        return horoscopeHistory.isReadTodayHoroscope == false;
    }

    protected async override UniTask OnClick()
    {
        HoroscopeHistory horoscopeHistory = GetHoroscopeHistory();

        if (horoscopeHistory == null) return;

        if (horoscopeHistory.isReadTodayHoroscope == false)
        {
            SaveDataManager.SaveData.exp += 5;
        }
        SaveDataManager.SaveData.horoscopeHistories[Key].isReadTodayHoroscope = true;
        await SaveDataManager.SaveAsync();
        ReturnLocalPushNotification.SetLocalPush();

        var constellation = SaveDataManager.SaveData.Constellation;
        await ScreenManager.Instance.Get<HoroscopeScreen>().Open(
            constellation,
            DateTime.Today,
            SaveDataManager.SaveData.GetCurrentCharacter());
    }
}
