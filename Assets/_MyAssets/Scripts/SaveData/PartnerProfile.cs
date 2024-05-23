using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace SaveDataObjects
{
    [Serializable]
    [JsonObject]
    public class PartnerProfile
    {
        public string name;
        public DateTime birthday;

    }
}