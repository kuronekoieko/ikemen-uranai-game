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
        public string time_id;
        public string time_name;
        public string priority;
        public string start;
        public string end;
    }
}