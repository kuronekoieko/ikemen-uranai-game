using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class ChargingProduct
    {
        public int id;
        public int JemCount;
        public int jpy;
    }
}

