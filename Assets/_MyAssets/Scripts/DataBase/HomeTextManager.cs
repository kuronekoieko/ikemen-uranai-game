using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataBase;
using UnityEngine;

public static class HomeTextManager
{

    public static HomeText GetHomeText(DateTime dateTime, HashSet<DateTime> holidays, HomeText[] homeTexts)
    {
        bool isHoliday = holidays.Any(holiday => holiday.Date == dateTime.Date);

        var group = homeTexts
            .Where(homeText =>
            {
                if (homeText.date.priority == 0) return true;
                if (DateTime.TryParse(homeText.date.date, out DateTime dateDT))
                {
                    return dateDT == dateTime.Date;
                }
                else
                {
                    return false;
                }
            })
            .Where(homeText =>
            {
                if (homeText.day.priority == 0) return true;
                return homeText.day.IsIncludeDay(dateTime.DayOfWeek, isHoliday);
            })
            .Where(homeText =>
            {
                if (homeText.time.priority == 0) return true;
                TimeSpan startTimeOfDay = homeText.time.StartTimeOfDay();
                TimeSpan endTimeOfDay = homeText.time.EndTimeOfDay();

                // Debug.Log(startTimeOfDay + " - " + endTimeOfDay);

                return startTimeOfDay <= dateTime.TimeOfDay && dateTime.TimeOfDay <= endTimeOfDay;
            })
            .GroupBy(homeText => homeText.Priority)
            .OrderByDescending(group => group.Key)
            .FirstOrDefault();

        if (group == null)
        {
            Debug.LogError("ホーム会話がみつかりません");
            return null;
        }

        foreach (var homeText in group.ToArray())
        {
            // DebugUtils.LogJson(homeText);
            // Debug.Log(homeText.date.date_name + " " + homeText.day.day_name + " " + homeText.time.time_name + " " + homeText.Priority);
        }
        //var randomHomeText = group.ToArray().GetRandom();

        return group.ToArray().GetRandom();
    }
}
