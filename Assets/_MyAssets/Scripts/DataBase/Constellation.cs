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

        public DateTime StartDT => start.ToDateTime();
        public DateTime EndDT => end.ToDateTime();


        DateTime ToDateTime(string value)
        {
            DateTime dateTime = value.ToDateTime();

            int lastIndex = CSVManager.Instance.Constellations.Length - 1;
            Constellation lastConstellation = CSVManager.Instance.Constellations[lastIndex];

            DateTime lastConstellationEndDT = lastConstellation.end.ToDateTime();

            Debug.Log(dateTime + " " + lastConstellationEndDT.Date + " " + (dateTime.Date <= lastConstellationEndDT.Date));
            if (dateTime.Date <= lastConstellationEndDT.Date)
            {
                dateTime = dateTime.AddYears(1);
            }
            return dateTime;
        }




    }
}