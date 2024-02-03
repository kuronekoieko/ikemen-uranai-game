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
    }
}