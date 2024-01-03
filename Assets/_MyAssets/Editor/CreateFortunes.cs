using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;
using Cysharp.Threading.Tasks;
using System;
using Newtonsoft.Json;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Reflection;

public class CreateFortunes
{

    [MenuItem("MyTool/Create Fortunes")]
    static async void Start()
    {
        Debug.Log("計算開始");

        var Constellations = await CSVManager.Instance.DeserializeAsync<Constellation>("Constellation");
        var LuckyItems = await CSVManager.Instance.DeserializeAsync<LuckyItem>("Fortunes/LuckyItems");
        var LuckyColors = await CSVManager.Instance.DeserializeAsync<LuckyColor>("Fortunes/LuckyColors");

        var LuckyItemList = LuckyItems.Select(luckyItem => luckyItem.name).ToList();
        var LuckyColorList = LuckyColors.Select(luckyColor => luckyColor.name).ToList();
        var rankList = Enumerable.Range(1, 12).ToList();
        var msgNoList = Enumerable.Range(1, 20).ToList();
        var dateTimes = GenerateDateList(2);

        var fortunes = new List<Fortune>();
        var dailyFortunesList = new List<List<Fortune>>();


        foreach (var dateTime in dateTimes)
        {
            var luckyItems = new List<string>(LuckyItemList);
            var luckyColors = new List<string>(LuckyColorList);
            var ranks = new List<int>(rankList);
            var msgNos = new List<int>(msgNoList);
            var dailyFortunes = new List<Fortune>();
            var beforeDailyFortunes = dailyFortunesList.LastOrDefault();


            foreach (var constellation in Constellations)
            {
                Fortune fortune = new()
                {
                    date_time = dateTime.ToStringDate(),
                    constellation_id = constellation.id,
                    rank = GetBeforeRank(beforeDailyFortunes, constellation.id),
                    item = PopRandomWithoutLast7Days(luckyItems, dailyFortunesList, constellation.id),
                    color = PopRandomWithoutLast7Days(luckyColors, dailyFortunesList, constellation.id),
                    msg_id = msgNos.PopRandom(),
                };
                dailyFortunes.Add(fortune);
            }

            dailyFortunes = dailyFortunes.OrderBy(f => f.rank).ToList();
            List<Fortune> high = dailyFortunes.Where(f => f.rank <= 3);
            List<Fortune> mid = dailyFortunes.Where(f => 4 <= f.rank && f.rank <= 9);
            List<Fortune> low = dailyFortunes.Where(f => 10 <= f.rank);

            dailyFortunes = new();
            dailyFortunes.AddRange(high);
            dailyFortunes.AddRange(low);
            dailyFortunes.AddRange(mid);

            foreach (var dailyFortune in dailyFortunes)
            {
                dailyFortune.rank = PopRandomRank(dailyFortune.rank, ranks);
                if (dailyFortune.rank == 0) DebugUtils.LogJsonError("順位が異常", dailyFortune);
            }

            dailyFortunes = dailyFortunes.OrderBy(f => f.constellation_id).ToList();
            fortunes.AddRange(dailyFortunes);
            dailyFortunesList.Add(dailyFortunes);

            if (dateTime.Day == DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
            {
                await UniTask.DelayFrame(1);
                var progress = (float)fortunes.Count / (float)(dateTimes.Count * 12) * 10f;
                Debug.Log("計算中 " + Mathf.FloorToInt(progress) + "%");
            }
        }
        Save("Fortunes", fortunes);
    }

    static string PopRandomWithoutLast7Days(List<string> luckyItems, List<List<Fortune>> dailyFortunesList, string constellationId)
    {
        if (dailyFortunesList.Count == 0) return luckyItems.PopRandom();

        var last7DaysDailyFortunesList = dailyFortunesList.ReverseList().Take(7).ToList().ReverseList();
        var last7DaysLuckyItems = last7DaysDailyFortunesList
            .Select(dailyFortunes => dailyFortunes.FirstOrDefault(dailyFortune => dailyFortune.constellation_id == constellationId))
            .Where(dailyFortune => dailyFortune != null)
            .Select(dailyFortune => dailyFortune.item)
            .ToList();

        bool invalid = luckyItems.All(luckyItem => last7DaysLuckyItems.Contains(luckyItem));
        if (invalid)
        {
            Debug.LogError("アイテムがありません");
            return "";
        }

        string luckyItem = luckyItems.PopRandom(isRetry: luckyItem => last7DaysLuckyItems.Contains(luckyItem));

        return luckyItem;
    }


    static int GetBeforeRank(List<Fortune> beforeDailyFortunes, string constellationId)
    {
        if (beforeDailyFortunes == null) return 0;
        foreach (var fortune in beforeDailyFortunes)
        {
            if (fortune.constellation_id == constellationId)
            {
                return fortune.rank;
            }
        }

        return 0;
    }

    static int PopRandomRank(int beforeRank, List<int> ranks)
    {
        if (ranks.Count == 0)
        {
            // Debug.Log("要素数０");
            return default;
        }

        if (beforeRank == 0)
        {
            return ranks.PopRandom();
        }

        int rank = ranks.GetRandom();
        if (beforeRank <= 3)
        {
            if (ranks.All(rank => rank <= 3))
            {
                Debug.LogError("順位が異常");
                return 0;
            }
            rank = ranks.PopRandom(isRetry: rank => rank <= 3);
            // Debug.Log("前が3位いないのとき " + rank);
            return rank;
        }

        if (10 <= beforeRank)
        {
            if (ranks.All(rank => 10 <= rank))
            {
                Debug.LogError("順位が異常");
                return 0;
            }
            rank = ranks.PopRandom(isRetry: rank => 10 <= rank);
            // Debug.Log("前が10位以上のとき " + rank);
            return rank;
        }

        rank = ranks.PopRandom();
        // Debug.Log("その他 " + rank);
        return rank;
    }

    static List<DateTime> GenerateDateList(int days)
    {
        List<DateTime> dateList = new();

        // 現在の日付を取得
        DateTime currentDate = new(2023, 12, 1);

        // 5年分の日付を生成してリストに追加
        for (int i = 0; i < days; i++)
        {
            dateList.Add(currentDate.AddDays(i));
        }

        return dateList;
    }

    static void Save(string fileName, List<Fortune> fortunes)
    {
        Debug.Log("書き込み開始");

        string path = Application.dataPath + @"/_MyAssets/CSV/Fortunes/" + fileName + ".csv";
        using StreamWriter sw = File.CreateText(path);

        string titleLine = "";


        // クラスの変数を配列で取得
        FieldInfo[] fields = typeof(Fortune).GetFields();

        // 配列を反復処理して各変数にアクセス
        foreach (FieldInfo field in fields)
        {
            //object value = field.GetValue(instance);
            //Debug.Log(field.Name + ": " + value);
            titleLine += field.Name + ",";
        }



        sw.WriteLine(titleLine);


        for (int i = 0; i < fortunes.Count; i++)
        {
            var fortune = fortunes[i];
            string line = "";
            foreach (FieldInfo field in fields)
            {
                line += field.GetValue(fortune) + ",";
            }

            sw.WriteLine(line);
        }


        AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);
    }


    [Serializable]
    [JsonObject]
    public class LuckyItem
    {
        public string id;
        public string name;
    }

    [Serializable]
    [JsonObject]
    public class LuckyColor
    {
        public string id;
        public string name;
    }
}
