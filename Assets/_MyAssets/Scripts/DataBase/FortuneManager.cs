using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataBase;
using Sirenix.Utilities;
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
            fortune = CSVManager.Fortunes
                 .Where(f => f.date_time == dateTime.ToDateKey())
                 .FirstOrDefault(f => f.constellation_id == constellation_id);
        }

        if (fortunesDic.ContainsKey(key) == false)
        {
            fortunesDic[key] = fortune;
        }

        return fortune;
    }

    public static LuckyItem GetLuckyItem(int lucky_item_id)
    {
        LuckyItem luckyItem = CSVManager.LuckyItems.FirstOrDefault(luckyItem => luckyItem.id == lucky_item_id);
        var stuckLuckyItems = new List<LuckyItem>(CSVManager.LuckyItems);

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

    public static LuckyColor GetLuckyColor(int lucky_item_id)
    {
        LuckyColor luckyColor = CSVManager.LuckyColors.FirstOrDefault(luckyColor => luckyColor.id == lucky_item_id);
        var stuckLuckyColors = new List<LuckyColor>(CSVManager.LuckyColors);

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


    public static string GetFortuneMessage(SaveDataObjects.Character saveDataCharacter, int rank)
    {
        int characterId = (saveDataCharacter != null) ? saveDataCharacter.id : 0;

        var fortuneMessage = CSVManager.FortuneMessages
            .Where(f => f.character_id == characterId)
            .FirstOrDefault(f => f.rank == rank);

        // idが存在しないときは、デフォルトキャラのメッセージ
        // TODO: 仕様変わる可能性あり
        if (fortuneMessage == null)
        {
            fortuneMessage = CSVManager.FortuneMessages
                .FirstOrDefault(f => f.rank == rank);
        }

        if (fortuneMessage.messages.IsNullOrEmpty())
        {
            return null;
        }
        return fortuneMessage.messages.GetRandom();
    }

}
