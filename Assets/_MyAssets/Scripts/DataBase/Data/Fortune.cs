using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class Fortune
    {
        public string date_time;
        public string constellation_id;
        public int rank;
        public int lucky_item_id;
        public int lucky_color_id;
        public int msg_id;
    }
}
