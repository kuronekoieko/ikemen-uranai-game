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
        string key = dateTime.ToDateKey() + "_" + constellation_id;
        bool success = fortunesDic.TryGetValue(key, out Fortune fortune);
        if (success == false)
        {
            fortune = CSVManager.Instance.Fortunes
                 .Where(f => f.date_time == dateTime.ToDateKey())
                 .FirstOrDefault(f => f.constellation_id == constellation_id);
        }

        if (fortunesDic.ContainsKey(key) == false)
        {
            fortunesDic[key] = fortune;
        }

        return fortune;
    }

    public static LuckyItem GetLuckyItem(string lucky_item_id)
    {
        LuckyItem luckyItem = CSVManager.Instance.LuckyItems.FirstOrDefault(luckyItem => luckyItem.id == lucky_item_id);
        var stuckLuckyItems = new List<LuckyItem>(CSVManager.Instance.LuckyItems);

        while (luckyItem == null || string.IsNullOrEmpty(luckyItem.name))
        {
            luckyItem = stuckLuckyItems.PopRandom();
            if (stuckLuckyItems.Count <= 0)
            {
                return null;
            }
        }

        return luckyItem;
    }

    public static LuckyColor GetLuckyColor(string lucky_item_id)
    {
        LuckyColor luckyColor = CSVManager.Instance.LuckyColors.FirstOrDefault(luckyColor => luckyColor.id == lucky_item_id);
        var stuckLuckyColors = new List<LuckyColor>(CSVManager.Instance.LuckyColors);

        while (luckyColor == null || string.IsNullOrEmpty(luckyColor.name))
        {
            luckyColor = stuckLuckyColors.PopRandom();
            if (stuckLuckyColors.Count <= 0)
            {
                return null;
            }
        }

        return luckyColor;
    }

}
