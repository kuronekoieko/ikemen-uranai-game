using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]
    public class Constellation
    {
        public string id;
        public string name;
        public string latin_name;
        public string start;
        public string end;

        public DateTime? StartDT => start.ToNullableDateTime();
        public DateTime? EndDT => end.ToNullableDateTime();
    }
}