using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
namespace SaveDataObjects
{
    [Serializable]
    [JsonObject]
    public class Notification
    {
        public bool isOnTodayHoroscope = true;
        public bool isOnNextDayHoroscope = true;
        public bool isOnOthers = true;
    }
}