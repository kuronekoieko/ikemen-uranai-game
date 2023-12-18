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
        public string name_jp;
        public string voice_actor_jp;// 変数名はcsvのキーと同じにする
    }
}