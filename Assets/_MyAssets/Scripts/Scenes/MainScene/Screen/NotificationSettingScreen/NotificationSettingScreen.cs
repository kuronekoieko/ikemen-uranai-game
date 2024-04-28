using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class NotificationSettingScreen : BaseScreen
{
    [SerializeField] NotificationSettingPool notificationSettingPool;
    [SerializeField] TextMeshProUGUI titleText;



    public override void OnStart()
    {
        base.OnStart();
        notificationSettingPool.OnStart();

        var notificationSettingObjs = new List<NotificationSettingObj>();
        notificationSettingObjs.Add(new()
        {
            title = "明日の星座占いの通知(毎日、夜の21:00)",
            onValueChanged = OnValueChanged_NextDay,
            isOn = SaveDataManager.SaveData.notification.isOnNextDayHoroscope
        });
        notificationSettingObjs.Add(new()
        {
            title = "今日の星座占いの通知(毎日、朝の8:30)",
            onValueChanged = OnValueChanged_Today,
            isOn = SaveDataManager.SaveData.notification.isOnTodayHoroscope
        });
        notificationSettingObjs.Add(new()
        {
            title = "その他(その他の通知)",
            onValueChanged = OnValueChanged_Others,
            isOn = SaveDataManager.SaveData.notification.isOnOthers
        });

        notificationSettingPool.Show(notificationSettingObjs.ToArray());
    }

    public void Open(string title)
    {
        titleText.text = title;
        base.Open();
    }

    public override UniTask Close()
    {
        base.Close();
        return UniTask.DelayFrame(0);
    }

    async void OnValueChanged_NextDay(bool isOn)
    {
        SaveDataManager.SaveData.notification.isOnNextDayHoroscope = isOn;
        bool success = await SaveDataManager.SaveAsync();
        if (success) LocalPushNotificationManager.SetLocalPush();
    }
    async void OnValueChanged_Today(bool isOn)
    {
        SaveDataManager.SaveData.notification.isOnTodayHoroscope = isOn;
        bool success = await SaveDataManager.SaveAsync();
        if (success) LocalPushNotificationManager.SetLocalPush();
    }
    async void OnValueChanged_Others(bool isOn)
    {
        SaveDataManager.SaveData.notification.isOnOthers = isOn;
        bool success = await SaveDataManager.SaveAsync();
        if (success) LocalPushNotificationManager.SetLocalPush();
    }

}
