using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace SaveDataObjects
{
    [Serializable]
    [JsonObject]
    public class Character
    {
        public string id;
        public int level = 1;
        public int exp = 200;
    }
}