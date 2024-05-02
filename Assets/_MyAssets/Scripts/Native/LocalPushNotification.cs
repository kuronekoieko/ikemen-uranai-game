#if UNITY_ANDROID && !UNITY_EDITOR
using Unity.Notifications.Android;
#endif
#if UNITY_IOS && !UNITY_EDITOR
using Unity.Notifications.iOS;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// https://marumaro7.hatenablog.com/entry/localpush
/// </summary>
/// // ローカルプッシュ通知送信クラス
public static class LocalPushNotification
{
    // Androidで使用するプッシュ通知用のチャンネルを登録する。    
    public static void RegisterChannel(string cannelId, string title, string description)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // チャンネルの登録
        var channel = new AndroidNotificationChannel()
        {
            Id = cannelId,
            Name = title,
            Importance = Importance.High,//ドキュメント　重要度を設定するを参照　https://developer.android.com/training/notify-user/channels?hl=ja
            Description = description,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
#endif
    }





    /// 通知をすべてクリアーします。   
    public static void AllClear()
    {
        Debug.Log("LocalPushNotification: AllClear");
#if UNITY_ANDROID && !UNITY_EDITOR
        // Androidの通知をすべて削除します。
        AndroidNotificationCenter.CancelAllScheduledNotifications();
        AndroidNotificationCenter.CancelAllNotifications();
#endif
#if UNITY_IOS && !UNITY_EDITOR
        // iOSの通知をすべて削除します。
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
        // バッジを消します。
        iOSNotificationCenter.ApplicationBadge = 0;
#endif
    }

    public static bool Enabled
    {
        get
        {
#if UNITY_IOS && !UNITY_EDITOR
            return UnityEngine.iOS.NotificationServices.enabledNotificationTypes != UnityEngine.iOS.NotificationType.None;
#elif UNITY_ANDROID && !UNITY_EDITOR
            string notificationStatusClass = Application.identifier + ".notification.NotificationStatusChecker";
            var notificationStatusChecker = new AndroidJavaObject(notificationStatusClass);
            var areNotificationsEnabled = notificationStatusChecker.Call<bool>("areNotificationsEnabled");
            return areNotificationsEnabled;
#endif
            return true;
        }
    }


    public static IEnumerator RequestAuthorization()
    {
#if UNITY_IOS && !UNITY_EDITOR
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
#else
        yield return null;

#endif
    }


    public static void AddSchedules(List<Config> configs)
    {
        // 一応100こまで
        var orderedConfigs = configs.
            OrderBy(config => config.targetDateTime)
            .Where(config => config.targetDateTime > DateTime.Now)
            //.Where(config => config.targetDateTime < DateTime.Now.AddYears(4))
            .ToList();
        int badgeCount = 0;
        foreach (var config in orderedConfigs)
        {
            badgeCount++;
            // DebugUtils.LogJson("LocalPushNotification: ", config);
            // Debug.Log("LocalPushNotification: " + badgeCount + " " + config.targetDateTime + " " + config.message);
            AddSchedule(config, badgeCount);
        }

        //  Config first = configs[0];
        // Config last_1 = configs[configs.Count - 2];
        //  Config last = configs[configs.Count - 1];

        // Debug.Log((last_1.targetDateTime - first.targetDateTime).TotalDays);
        // Debug.Log(last_1.targetDateTime + " " + first.targetDateTime);

        //Debug.Log((last.targetDateTime - first.targetDateTime).TotalDays);
        //Debug.Log(last.targetDateTime + " " + first.targetDateTime);

    }

    static void AddSchedule(Config config, int badgeCount)
    {
        TimeSpan timeSpan = config.targetDateTime - DateTime.Now;
        int sec = (int)timeSpan.TotalSeconds;
        if (sec <= 0) return;
        Config_Sec config_Sec = new()
        {
            title = config.title,
            message = config.message,
            elapsedTime = sec,
            badgeCount = badgeCount,
            cannelId = config.cannelId,
        };
        AddScheduleSec(config_Sec);
    }

    public static void AddScheduleDays(string title, string message, int elapsedDay, string cannelId)
    {
        int secPerDay = 60 * 60 * 24;
        Config_Sec config = new()
        {
            title = title,
            message = message,
            elapsedTime = secPerDay * elapsedDay,
            cannelId = cannelId,
        };
        AddScheduleSec(config);
    }


    // プッシュ通知を登録します。    
    static void AddScheduleSec(Config_Sec config)
    {
        //  DebugUtils.LogJson(config);

#if UNITY_ANDROID && !UNITY_EDITOR
        SetAndroidNotification(config.title, config.message, config.badgeCount, config.elapsedTime, config.cannelId);
#endif
#if UNITY_IOS && !UNITY_EDITOR
        SetIOSNotification(config.title, config.message, config.badgeCount, config.elapsedTime);
#endif
    }



#if UNITY_IOS && !UNITY_EDITOR
    // 通知を登録(iOS)
    static private void SetIOSNotification(string title, string message, int badgeCount, int elapsedTime)
    {
        // 通知を作成
        iOSNotificationCenter.ScheduleNotification(new iOSNotification()
        {
            //プッシュ通知を個別に取り消しなどをする場合はこのIdentifierを使用します。(未検証)
            // Identifierで通知を区別しているため、同じだと上書きされて前のが消える
            Identifier = $"_notification_{badgeCount}",
            Title = title,
            Body = message,
            ShowInForeground = false,
            Badge = badgeCount,
            Trigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, 0, elapsedTime),
                Repeats = false
            }
        });
    }
#endif



#if UNITY_ANDROID && !UNITY_EDITOR

    // 通知を登録(Android)   
    static private void SetAndroidNotification(string title, string message, int badgeCount, int elapsedTime, string cannelId)
    {
        // 通知を作成します。
        var notification = new AndroidNotification
        {
            Title = title,
            Text = message,
            Number = badgeCount,

            //Androidのアイコンを設定
            SmallIcon = "ic_stat_notify_small",//どの画像を使用するかアイコンのIdentifierを指定　指定したIdentifierが見つからない場合アプリアイコンになる。
            LargeIcon = "ic_stat_notify_large",//どの画像を使用するかアイコンのIdentifierを指定　指定したIdentifierが見つからない場合アプリアイコンになる。
            FireTime = DateTime.Now.AddSeconds(elapsedTime)
        };

        // 通知を送信します。
        AndroidNotificationCenter.SendNotification(notification, cannelId);

    }
#endif

    public class Config
    {
        public string title;
        public string message;
        public string cannelId;
        public DateTime targetDateTime;
    }

    public class Config_Sec
    {
        public string title;
        public string message;
        public string cannelId;
        public int badgeCount;
        public int elapsedTime;
    }

}