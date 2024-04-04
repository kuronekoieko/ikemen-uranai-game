using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SaveDataObjects;
using Cysharp.Threading.Tasks;


public class TodayHoroscopeButton : BaseHoroscopeButton
{

    public override void OnStart()
    {
        base.OnStart();
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
        return horoscopeHistory.isReadTodayHoroscope == false;
    }

    protected async override UniTask OnClick()
    {
        HoroscopeHistory horoscopeHistory = await GetHoroscopeHistory();

        if (horoscopeHistory == null) return;

        if (horoscopeHistory.isReadTodayHoroscope == false)
        {
            SaveDataManager.SaveData.exp += 5;
        }
        SaveDataManager.SaveData.horoscopeHistories[Key].isReadTodayHoroscope = true;
        await SaveDataManager.SaveAsync();
        ReturnLocalPushNotification.SetLocalPush();
        Kill();

        var constellation = SaveDataManager.SaveData.Constellation;
        await ScreenManager.Instance.Get<HoroscopeScreen>().Open(
            constellation,
            DateTime.Today,
            SaveDataManager.SaveData.GetCurrentCharacter());
    }
}
