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
        public string id;
        public string chara_id;
        public string date_id;
        public string day_id;
        public string time_id;
        public string text_id;
    }
}