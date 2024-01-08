using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class FortuneMessage
    {
        public int character_id;
        public int rank;
        public string[] messages;

    }
}