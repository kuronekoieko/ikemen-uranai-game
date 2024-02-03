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
        public string[] keys;
    }
}