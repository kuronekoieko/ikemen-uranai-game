using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DataBase
{

    [Serializable]
    public class Character
    {
        public string id;
        public string name;
        public string voice_actor;// 変数名はcsvのキーと同じにする
    }
}