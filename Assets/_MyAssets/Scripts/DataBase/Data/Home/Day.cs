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
        public string day_id;
        public string day_name;
        public string priority;
        public string keys;
    }
}