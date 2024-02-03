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
        public string day_id;
        public string date_name;
        public string priority;
        public string date;
    }
}