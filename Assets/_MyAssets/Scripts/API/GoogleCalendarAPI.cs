using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using Cysharp.Threading.Tasks;
using System.Net;
using System.Linq;


public static class GoogleCalendarAPI
{
    static readonly Dictionary<int, HashSet<DateTime>> holidaysDic = new();

    public static async UniTask<HashSet<DateTime>> GetHolidaysAsync(int year)
    {
        Debug.Log("googleカレンダー 初期化開始");

        bool exists = holidaysDic.TryGetValue(year, out HashSet<DateTime> holidays);
        if (exists) return holidays;
        holidays = await RequestHolidaysAsync(year);

        // 祝日が取れなくても、最悪平日として会話を続ける方が止まるより良い
        if (holidays == null) return new();
        if (!holidaysDic.ContainsKey(year))
        {
            holidaysDic.Add(year, holidays);
        }
        return holidays;
    }

    static async UniTask<HashSet<DateTime>> RequestHolidaysAsync(int year)
    {
        Debug.Log("googleカレンダー アクセス開始");
        var key = FirebaseRemoteConfigManager.GetString(FirebaseRemoteConfigManager.Key.google_calender_api_key);
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("GoogleCalendarAPI key: " + key);
            return null;
        }
        var holidaysId = "japanese__ja@holiday.calendar.google.com";
        var startDate = new DateTime(year, 1, 1).ToString("yyyy-MM-dd") + "T00%3A00%3A00.000Z";
        var endDate = new DateTime(year, 12, 31).ToString("yyyy-MM-dd") + "T00%3A00%3A00.000Z";
        var maxCount = 30;

        var url = $"https://www.googleapis.com/calendar/v3/calendars/{holidaysId}/events?key={key}&timeMin={startDate}&timeMax={endDate}&maxResults={maxCount}&orderBy=startTime&singleEvents=true";

        var result = await API.Get(url, timeoutSec: 3);

        switch (result.status)
        {
            case API.Result.Status.Success:
                var json = result.responseJson;
                //DebugUtils.LogJson(json);
                if (json == null) return null;
                var o = JObject.Parse(json);
                var days = o["items"].Select(i =>
                {
                    DateTime.TryParse(i["start"]["date"].ToString(), out DateTime dateTime);
                    return dateTime;
                });
                Debug.Log("googleカレンダー 成功");

                return new HashSet<DateTime>(days);
            case API.Result.Status.Error:
                Debug.Log("googleカレンダー エラー");

                return null;
            case API.Result.Status.Canceled:
                Debug.Log("googleカレンダー キャンセル");

                return null;
            default:
                return null;
        }
    }
}
