using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class HomeText
    {
        public int id;
        public int chara_id;
        public int date_id;
        public int day_id;
        public int time_id;
        public int text_id;
        public Date date = new();
        public Day day = new();
        public Time time = new();
        public string FileName => "chara" + chara_id.ToString("D3") + "-text" + text_id.ToString("D3");
        public int Priority => date.priority + day.priority + time.priority;
    }
}