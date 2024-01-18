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

            LocalPushNotification.AddScheduleDays(
                Application.productName,
                message,
                1,
                day,
                "001"
            );
        }

        for (int i = 1; i < 100; i++)
        {
            string message = messages[0];
            int day = 87 + 29 * i;
            LocalPushNotification.AddScheduleDays(
                Application.productName,
                message,
                1,
                day,
                "001"
            );
        }
    }

}
