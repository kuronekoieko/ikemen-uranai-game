using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationSettingScreen : BaseScreen
{
    [SerializeField] NotificationSettingPool notificationSettingPool;



    public override void OnStart()
    {
        base.OnStart();
        notificationSettingPool.OnStart();

        var notificationSettingObjs = new List<NotificationSettingObj>();
        notificationSettingObjs.Add(new()
        {
            title = "明日の運勢の通知(毎日、朝の8:30)",
            onValueChanged = OnValueChanged_NextDay,
            isOn = SaveDataManager.SaveData.notification.isOnNextDayHoroscope
        });
        notificationSettingObjs.Add(new()
        {
            title = "今日の運勢の通知(毎日、朝の8:30)",
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

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    void OnValueChanged_NextDay(bool isOn)
    {
        SaveDataManager.SaveData.notification.isOnNextDayHoroscope = isOn;
        SaveDataManager.Save();
    }
    void OnValueChanged_Today(bool isOn)
    {
        SaveDataManager.SaveData.notification.isOnTodayHoroscope = isOn;
        SaveDataManager.Save();
    }
    void OnValueChanged_Others(bool isOn)
    {
        SaveDataManager.SaveData.notification.isOnOthers = isOn;
        SaveDataManager.Save();
        if (isOn)
        {
            ReturnLocalPushNotification.SetLocalPush();
        }
        else
        {
            LocalPushNotification.AllClear();
        }
    }

}
