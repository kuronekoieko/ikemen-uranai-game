using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaveDataObjects;
using UnityEngine;

public static class LocalPushNotificationManager
{
    public static void SetLocalPush()
    {
        //ログインしたらリセット
        LocalPushNotification.AllClear();
        Debug.Log("LocalPushNotification: 通知設定");


        // iosは、badgeCountでそれそれの通知を区別しているため、badgeCountが同じだと上書きされて消える
        // badgeCountは、勝手に加算されるわけではなく、指定した数が表示される
        // 例)badgeCount=40のときは、アイコンに「40」が出る

        //Test();
        // return;


        var configs = new List<LocalPushNotification.Config>();

        string Key = DateTime.Today.ToDateKey();


        if (SaveDataManager.SaveData.horoscopeHistories.TryGetValue(Key, out HoroscopeHistory horoscopeHistory))
        {
            var todayConfigs = GetTodayConfigs(horoscopeHistory.isReadTodayHoroscope);
            configs.AddRange(todayConfigs);
            var nextDayConfigs = GetNextDayConfigs(horoscopeHistory.isReadNextDayHoroscope);
            configs.AddRange(nextDayConfigs);
        }

        var otherConfigs = GetOtherConfigs();
        configs.AddRange(otherConfigs);

        LocalPushNotification.AddSchedules(configs);
    }


    static List<LocalPushNotification.Config> GetTodayConfigs(bool isReadTodayHoroscope)
    {
        var configs = new List<LocalPushNotification.Config>();
        if (!SaveDataManager.SaveData.notification.isOnTodayHoroscope) return configs;

        for (int i = 0; i < 7; i++)
        {
            if (i == 0 && isReadTodayHoroscope) continue;

            DateTime morningDT = DateTime.Today.AddDays(i).AddHours(8).AddMinutes(30);

            LocalPushNotification.Config config = new()
            {
                title = Application.productName,
                message = "今日の運勢をチェックしよう！",
                targetDateTime = morningDT,
                cannelId = "001",
            };
            configs.Add(config);
            //  Debug.Log(morningDT);
        }

        return configs;
    }

    static List<LocalPushNotification.Config> GetNextDayConfigs(bool isReadNextDayHoroscope)
    {
        var configs = new List<LocalPushNotification.Config>();
        if (!SaveDataManager.SaveData.notification.isOnNextDayHoroscope) return configs;

        for (int i = 0; i < 7; i++)
        {
            if (i == 0 && isReadNextDayHoroscope) continue;

            DateTime nightDT = DateTime.Today.AddDays(i).AddHours(21);

            LocalPushNotification.Config config = new()
            {
                title = Application.productName,
                message = "明日の運勢が公開されたよ！",
                targetDateTime = nightDT,
                cannelId = "001",
            };
            configs.Add(config);
            // Debug.Log(nightDT);

        }


        return configs;
    }



    static List<LocalPushNotification.Config> GetOtherConfigs()
    {
        var configs = new List<LocalPushNotification.Config>();

        if (!SaveDataManager.SaveData.notification.isOnOthers) return configs;

        var character = CSVManager.Characters.FirstOrDefault(character => character.id == SaveDataManager.SaveData.currentCharacterId);
        character ??= CSVManager.Characters.FirstOrDefault();
        string characterName = "イケボ";
        if (character != null) characterName = character.name_jp;

        Dictionary<int, string> messages = new()
        {
            {7,$"アプリにログインして、運勢をチェックしよう！{characterName}が待ってるよ！"},
            {15,$"アプリにログインして、運勢をチェックしよう！{characterName}が待ってるよ！"},
            {29,$"アプリにログインして、運勢をチェックしよう！{characterName}が待ってるよ！"},
            {58,$"アプリにログインして、運勢をチェックしよう！{characterName}が待ってるよ！"},
            {87,$"アプリにログインして、運勢をチェックしよう！{characterName}が待ってるよ！"},
            {0,$"アプリに戻ってきてね。イケボが待ってるよ！の最後の文面。"},
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



        for (int i = 1; i < 2; i++)
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


        return configs;
    }



    static void Test()
    {
        int local_push_test_sec = FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.local_push_test_sec);
        int local_push_test_count = FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.local_push_test_count);
        int local_push_test_duration = FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.local_push_test_duration);
        string local_push_test_text = FirebaseRemoteConfigManager.GetString(FirebaseRemoteConfigManager.Key.local_push_test_text);

        var configs = new List<LocalPushNotification.Config>();


        for (int i = 0; i < local_push_test_count; i++)
        {
            LocalPushNotification.Config config = new()
            {
                title = Application.productName,
                message = i + " " + local_push_test_text,
                targetDateTime = DateTime.Now.AddSeconds(local_push_test_sec),
                cannelId = i.ToString("D3"),
            };
            configs.Add(config);
            local_push_test_sec += local_push_test_duration;
        }


        LocalPushNotification.AddSchedules(configs);


    }

}

