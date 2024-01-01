using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataBase;
using UnityEngine;

public class FortuneManager
{
    static readonly Dictionary<string, Fortune> fortunesDic = new();


    public static Fortune GetFortune(DateTime dateTime, string constellation_id)
    {
        string key = dateTime.ToStringDate() + "_" + constellation_id;
        bool success = fortunesDic.TryGetValue(key, out Fortune fortune);
        if (success == false)
        {
            fortune = CSVManager.Instance.Fortunes
                 .Where(f => f.date_time == dateTime.ToStringDate())
                 .FirstOrDefault(f => f.constellation_id == constellation_id);
        }

        if (fortunesDic.ContainsKey(key) == false)
        {
            fortunesDic[key] = fortune;
        }

        return fortune;
    }


}
