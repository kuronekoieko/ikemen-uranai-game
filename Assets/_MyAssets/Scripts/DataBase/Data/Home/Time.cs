using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class Time
    {
        public int time_id;
        public string time_name;
        public int priority;
        public string start;
        public string end;
        public TimeSpan StartTimeOfDay() => start.ToDateTime().TimeOfDay;
        public TimeSpan EndTimeOfDay()
        {
            if (TimeSpan.TryParse(end, out TimeSpan timeSpan))
            {
                // Debug.Log(end + " aaaaaaaaaaaaa " + timeSpan);
                return timeSpan;
            }
            else
            {
                // Debug.Log(end + " bbbbbbbbbb " + timeSpan);
                // 24時がパースできないので、ここで24時に変換する
                // その他の理由でパースできない場合も、endなので最大値で設定
                return new TimeSpan(24, 0, 0);
            }
        }
    }
}