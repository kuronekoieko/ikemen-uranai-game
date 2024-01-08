using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class Character
    {
        public int id;
        public string name_jp;
        public string voice_actor_jp;
        public string description;
    }
}