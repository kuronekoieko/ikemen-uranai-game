using System;
using System.Collections;
using System.Collections.Generic;
using SaveDataObjects;
using UnityEngine;

public static class ReturnLocalPushNotification
{
    public static void SetLocalPush()
    {
        //ログインしたらリセット
        LocalPushNotification.AllClear();
        Debug.Log("LocalPushNotification: 通知設定");


        // iosは、badgeCountでそれそれの通知を区別しているため、badgeCountが同じだと上書きされて消える
        // badgeCountは、勝手に加算されるわけではなく、指定した数が表示される
        // 例)badgeCount=40のときは、アイコンに「40」が出る

        // Test();
        // return;

        if (SaveDataManager.SaveData.notification.isOnOthers == false) return;

        var configs = new List<LocalPushNotification.Config>();

        string Key = DateTime.Today.ToDateKey();


        if (SaveDataManager.SaveData.horoscopeHistories.TryGetValue(Key, out HoroscopeHistory horoscopeHistory))
        {
            bool isReadTodayHoroscope = horoscopeHistory.isReadTodayHoroscope;
            bool isReadNextDayHoroscope = horoscopeHistory.isReadNextDayHoroscope;

            for (int i = 0; i < 7; i++)
            {
                if (i == 0 && isReadTodayHoroscope) continue;

                DateTime morningDT = DateTime.Today.AddDays(i).AddHours(8).AddMinutes(30);

                LocalPushNotification.Config config = new()
                {
                    title = Application.productName,
                    message = "今日の運勢をチェックしてね",
                    targetDateTime = morningDT,
                    cannelId = "001",
                };
                configs.Add(config);
                //  Debug.Log(morningDT);
            }

            for (int i = 0; i < 7; i++)
            {
                if (i == 0 && isReadNextDayHoroscope) continue;

                DateTime nightDT = DateTime.Today.AddDays(i).AddHours(21);

                LocalPushNotification.Config config = new()
                {
                    title = Application.productName,
                    message = "明日の運勢が公開されたよ",
                    targetDateTime = nightDT,
                    cannelId = "001",
                };
                configs.Add(config);
                // Debug.Log(nightDT);

            }
        }


        Dictionary<int, string> messages = new()
        {
            {7,"アプリに戻ってきてね。イケボが待ってるよ！の文面1"},
            {15,"アプリに戻ってきてね。イケボが待ってるよ！の文面2"},
            {29,"アプリに戻ってきてね。イケボが待ってるよ！の文面3"},
            {58,"アプリに戻ってきてね。イケボが待ってるよ！の文面4"},
            {87,"アプリに戻ってきてね。イケボが待ってるよ！の文面5"},
            {0,"アプリに戻ってきてね。イケボが待ってるよ！の文面の最後"},
        };

        foreach (var pair in messages)
        {
            string message = pair.Value;
            int day = pair.Key;
            if (day == 0) break;
            DateTime dateTime = DateTime.Today.AddDays(day).AddHours(8).AddMinutes(30);

            LocalPushNotification.Config config = new()
            {
                title = Application.productName,
                message = message,
                targetDateTime = dateTime,
                cannelId = "001",
            };
            configs.Add(config);
            //  Debug.Log(dateTime);
        }

        for (int i = 1; i < 100; i++)
        {
            string message = messages[0];
            int day = 87 + 29 * i;
            DateTime dateTime = DateTime.Today.AddDays(day).AddHours(8).AddMinutes(30);

            LocalPushNotification.Config config = new()
            {
                title = Application.productName,
                message = message,
                targetDateTime = dateTime,
                cannelId = "001",
            };
            configs.Add(config);

            //  Debug.Log(dateTime);
        }

        LocalPushNotification.AddSchedules(configs);
    }

    static void Test()
    {
        int local_push_test_sec = FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.local_push_test_sec);
        int local_push_test_count = FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.local_push_test_count);
        int local_push_test_duration = FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.local_push_test_duration);
        var configs = new List<LocalPushNotification.Config>();


        for (int i = 0; i < local_push_test_count; i++)
        {
            LocalPushNotification.Config config = new()
            {
                title = Application.productName,
                message = "てすと " + i,
                targetDateTime = DateTime.Now.AddSeconds(local_push_test_sec),
                cannelId = i.ToString("D3"),
            };
            configs.Add(config);
            local_push_test_sec += local_push_test_duration;
        }


        LocalPushNotification.AddSchedules(configs);


    }

}
