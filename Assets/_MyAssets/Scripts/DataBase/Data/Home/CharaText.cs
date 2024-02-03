using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]

    public class CharaText
    {
        public int text_id;
        public string line;
        public string text;
        public string animation_id;
        public string voice_id;
    }
}
