using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using System;
using Cysharp.Threading.Tasks;
using System.Net;
using System.Linq;


public class GoogleCalendarAPI
{
    public static async UniTask<HashSet<DateTime>> GetHolidaysAsync(int year)
    {
        var key = "";
        var holidaysId = "japanese__ja@holiday.calendar.google.com";
        var startDate = new DateTime(year, 1, 1).ToString("yyyy-MM-dd") + "T00%3A00%3A00.000Z";
        var endDate = new DateTime(year, 12, 31).ToString("yyyy-MM-dd") + "T00%3A00%3A00.000Z";
        var maxCount = 30;

        var url = $"https://www.googleapis.com/calendar/v3/calendars/{holidaysId}/events?key={key}&timeMin={startDate}&timeMax={endDate}&maxResults={maxCount}&orderBy=startTime&singleEvents=true";
        var client = new WebClient() { Encoding = System.Text.Encoding.UTF8 };
        var json = await client.DownloadStringTaskAsync(url);
        client.Dispose();

        var o = JObject.Parse(json);
        var days = o["items"].Select(i =>
        {
            DateTime.TryParse(i["start"]["date"].ToString(), out DateTime dateTime);
            return dateTime;
        });
        return new HashSet<DateTime>(days);
    }
}
