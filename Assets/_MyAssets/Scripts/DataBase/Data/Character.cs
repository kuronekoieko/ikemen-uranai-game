using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    // 本当はjsonが良いが、プランナーがデータを作りやすくするためにcsvで
    [Serializable]
    [JsonObject]
    public class Character
    {
        public string id;
        public string name_jp;
        public string voice_actor_jp;
        public string description;
    }
}