using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace DataBase
{
    [Serializable]
    [JsonObject]

    public class PopupText
    {
        public int id;
        public string text;
        public string button_negative;
        public string button_positive;

        public static PopupText CreateDefault()
        {
            return new()
            {
                text = "エラーが発生しました。",
                button_positive = "OK",
            };
        }
    }
}