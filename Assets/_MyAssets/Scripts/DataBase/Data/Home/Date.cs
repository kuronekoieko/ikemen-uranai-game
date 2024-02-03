using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class Date
    {
        public int date_id;
        public string date_name;
        public int priority;
        public string date;
    }
}