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

        var LuckyItemList = LuckyItems.ToList();
        var LuckyColorList = LuckyColors.ToList();
        var rankList = Enumerable.Range(1, 12).ToList();
        var msgNoList = Enumerable.Range(1, 20).ToList();
        var dateTimes = GenerateDateList(365 * 10);
        // var dateTimes = GenerateDateList(3);

        var fortunes = new List<Fortune>();
        var beforeDailyFortunes = new List<Fortune>();


        foreach (var dateTime in dateTimes)
        {
            var luckyItems = new List<LuckyItem>(LuckyItemList);
            var luckyColors = new List<LuckyColor>(LuckyColorList);
            var ranks = new List<int>(rankList);
            var msgNos = new List<int>(msgNoList);
            var dailyFortunes = new List<Fortune>();

            foreach (var constellation in Constellations)
            {
                int beforeRank = GetBeforeRank(beforeDailyFortunes, dateTime, constellation.id);
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
                if (dailyFortune.rank == 0) DebugUtils.LogJson("順位が異常", dailyFortune);
            }

            dailyFortunes = dailyFortunes.OrderBy(f => f.constellation_id).ToList();
            fortunes.AddRange(dailyFortunes);
            beforeDailyFortunes = dailyFortunes;

            if (dateTime.Day == DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
            {
                await UniTask.DelayFrame(1);
                var progress = (float)fortunes.Count / (float)(dateTimes.Count * 12) * 10f;
                Debug.Log("計算中 " + Mathf.FloorToInt(progress) + "%");
            }
        }
        Save("Fortunes", fortunes);
    }

    static int GetBeforeRank(List<Fortune> beforeDailyFortunes, DateTime dateTime, string constellationId)
    {
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

    static async void Save(string fileName, List<Fortune> fortunes)
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
