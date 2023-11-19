#if UNITY_ANDROID && !UNITY_EDITOR
using Unity.Notifications.Android;
#endif
#if UNITY_IOS && !UNITY_EDITOR
using Unity.Notifications.iOS;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
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


    // プッシュ通知を登録します。    
    public static void AddSchedule(string title, string message, int badgeCount, int elapsedTime, string cannelId)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        SetAndroidNotification(title, message, badgeCount, elapsedTime, cannelId);
#endif
#if UNITY_IOS && !UNITY_EDITOR
        SetIOSNotification(title, message, badgeCount, elapsedTime);
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
}