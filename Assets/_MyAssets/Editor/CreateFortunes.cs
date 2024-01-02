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
    static Constellation[] Constellations;
    static LuckyItem[] LuckyItems;
    static LuckyColor[] LuckyColors;

    [MenuItem("MyTool/Create Fortunes")]
    static async void Start()
    {
        Constellations = await CSVManager.Instance.DeserializeAsync<Constellation>("Constellation");
        LuckyItems = await CSVManager.Instance.DeserializeAsync<LuckyItem>("Fortunes/LuckyItems");
        LuckyColors = await CSVManager.Instance.DeserializeAsync<LuckyColor>("Fortunes/LuckyColors");


        // var dateTimes = GenerateDateList(365 * 10);
        var dateTimes = GenerateDateList(2);

        List<Fortune> fortunes = new();

        foreach (var dateTime in dateTimes)
        {
            var luckyItems = LuckyItems.ToList();
            var luckyColors = LuckyColors.ToList();
            var ranks = Enumerable.Range(1, 12).ToList();
            var msgNos = Enumerable.Range(1, 20).ToList();
            List<Fortune> dailyFortunes = new();

            foreach (var constellation in Constellations)
            {
                int beforeRank = fortunes
                    .Where(f => f.date_time == dateTime.AddDays(-1).ToStringDate())
                    .Where(f => f.constellation_id == constellation.id)
                    .Select(f => f.rank)
                    .FirstOrDefault();
                Fortune fortune = new()
                {
                    date_time = dateTime.ToStringDate(),
                    constellation_id = constellation.id,
                    rank = beforeRank,
                    item = luckyItems.PopRandom().name,
                    color = luckyColors.PopRandom().name,
                    msg_id = msgNos.PopRandom(),
                };
                dailyFortunes.Add(fortune);
            }

            dailyFortunes = dailyFortunes.OrderBy(f => f.rank).ToList();
            var high = dailyFortunes.Where(f => f.rank <= 3);
            var mid = dailyFortunes.Where(f => 4 <= f.rank && f.rank <= 9);
            var low = dailyFortunes.Where(f => 10 <= f.rank);

            dailyFortunes = new();
            dailyFortunes.AddRange(high);
            dailyFortunes.AddRange(low);
            dailyFortunes.AddRange(mid);

            foreach (var dailyFortune in dailyFortunes)
            {
                dailyFortune.rank = PopRandomRank(dailyFortune.rank, ranks);
            }

            dailyFortunes = dailyFortunes.OrderBy(f => f.constellation_id).ToList();
            fortunes.AddRange(dailyFortunes);

            await UniTask.DelayFrame(1);
        }
        Save("Fortunes", fortunes);
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
            while (rank <= 3)
            {
                rank = ranks.GetRandom();
            }
            ranks.Remove(rank);
            // Debug.Log("前が3位いないのとき " + rank);

            return rank;
        }

        if (10 <= beforeRank)
        {
            while (10 <= rank)
            {
                rank = ranks.GetRandom();
            }
            ranks.Remove(rank);
            // Debug.Log("前が10位以上のとき " + rank);

            return rank;
        }

        rank = ranks.GetRandom();
        ranks.Remove(rank);
        Debug.Log("その他 " + rank);

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

        foreach (var fortune in fortunes)
        {
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
