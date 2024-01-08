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
        public int id;
        public int level = 1;
        public int exp = 200;

        public string IdToKey()
        {
            return "chara" + id.ToString("D3");
        }
    }
}