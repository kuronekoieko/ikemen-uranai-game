using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DataBase
{
    public class Constellation
    {
        public string id;
        public string name;
        public string start;
        public string end;

        public DateTime StartDT => Convert.ToDateTime(start);
        public DateTime EndDT => Convert.ToDateTime(end);
    }
}