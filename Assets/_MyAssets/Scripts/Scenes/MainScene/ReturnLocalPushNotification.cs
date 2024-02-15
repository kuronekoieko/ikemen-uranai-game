using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReturnLocalPushNotification
{
    public static void SetLocalPush()
    {
        //ログインしたらリセット
        LocalPushNotification.AllClear();

        if (SaveDataManager.SaveData.notification.isOnOthers == false) return;

        string Key = DateTime.Today.ToDateKey();
        bool isReadTodayHoroscope = SaveDataManager.SaveData.horoscopeHistories[Key].isReadTodayHoroscope;
        bool isReadNextDayHoroscope = SaveDataManager.SaveData.horoscopeHistories[Key].isReadNextDayHoroscope;


        for (int i = 0; i < 7; i++)
        {
            if (i == 0 && isReadTodayHoroscope) continue;

            DateTime morningDT = DateTime.Today.AddDays(i).AddHours(8).AddMinutes(30);
            LocalPushNotification.AddSchedule(
                   Application.productName,
                   "今日の運勢をチェックしてね",
                   1,
                   morningDT,
                   "001"
               );
        }

        for (int i = 0; i < 7; i++)
        {
            if (i == 0 && isReadNextDayHoroscope) continue;

            DateTime nightDT = DateTime.Today.AddDays(i).AddHours(21);
            LocalPushNotification.AddSchedule(
                   Application.productName,
                   "明日の運勢が公開されたよ",
                   1,
                   nightDT,
                   "001"
               );
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

            LocalPushNotification.AddSchedule(
                Application.productName,
                message,
                1,
                dateTime,
                "001"
            );
        }

        for (int i = 1; i < 100; i++)
        {
            string message = messages[0];
            int day = 87 + 29 * i;
            DateTime dateTime = DateTime.Today.AddDays(day).AddHours(8).AddMinutes(30);

            LocalPushNotification.AddSchedule(
                Application.productName,
                message,
                1,
                dateTime,
                "001"
            );
        }
    }

}
