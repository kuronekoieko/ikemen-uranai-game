using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class Day
    {
        public int day_id;
        public string day_name;
        public int priority;
        public bool is_sun;
        public bool is_mon;
        public bool is_tue;
        public bool is_wed;
        public bool is_thu;
        public bool is_fri;
        public bool is_sat;
        public bool is_hol;


        public bool IsIncludeDay(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Sunday => is_sun,
                DayOfWeek.Monday => is_mon,
                DayOfWeek.Tuesday => is_tue,
                DayOfWeek.Wednesday => is_wed,
                DayOfWeek.Thursday => is_thu,
                DayOfWeek.Friday => is_fri,
                DayOfWeek.Saturday => is_sat,
                _ => false,
            };
        }
    }
}