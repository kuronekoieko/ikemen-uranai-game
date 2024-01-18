using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace SaveDataObjects
{
    [Serializable]
    [JsonObject]
    public class HoroscopeHistory
    {
        public bool isReadTodayHoroscope = false;
        public bool isReadNextDayHoroscope = false;
    }
}
