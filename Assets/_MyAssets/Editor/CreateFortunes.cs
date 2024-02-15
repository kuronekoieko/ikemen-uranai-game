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
using UnityEngine.Events;

public class CreateFortunes
{
    [MenuItem("MyTool/Check Fortunes")]
    static async void A()
    {
        Debug.Log("計算開始");

        // TextAsset a = await AssetBundleLoader.LoadAssetAsync<TextAsset>("Assets/_MyAssets/CSV/Fortunes/Fortunes.csv");
        await CSVManager.InitializeAsync();
        var fortunes = CSVManager.Fortunes;
        var Constellations = CSVManager.Constellations;

        List<Test> tests = CreateTests(fortunes, Constellations);



        SaveTest("Test", tests);


    }

    static List<Test> CreateTests(Fortune[] fortunes, Constellation[] Constellations)
    {
        List<Test> tests = new();

        foreach (var constellation in Constellations)
        {
            Test test = new()
            {
                constellation_id = constellation.id,
            };
            foreach (var fortune in fortunes)
            {
                if (constellation.id == fortune.constellation_id)
                {
                    test.luckyItemIds.Add(fortune.lucky_item_id);
                    // test.luckyColorIds.Add(fortune.lucky_color_id);

                }

            }

            tests.Add(test);
        }

        foreach (var constellation in Constellations)
        {
            Test test = new()
            {
                constellation_id = constellation.id,
            };
            foreach (var fortune in fortunes)
            {
                if (constellation.id == fortune.constellation_id)
                {
                    test.luckyItemIds.Add(fortune.lucky_color_id);
                    // test.luckyColorIds.Add(fortune.lucky_color_id);

                }

            }

            tests.Add(test);
        }
        return tests;
    }

    static void SaveTest(string fileName, List<Test> list)
    {
        Debug.Log("書き込み開始");

        string path = Application.dataPath + @"/_MyAssets/CSV/Fortunes/" + fileName + ".csv";
        using StreamWriter sw = File.CreateText(path);

        string titleLine = "";


        // クラスの変数を配列で取得
        FieldInfo[] fields = typeof(Test).GetFields();

        // 配列を反復処理して各変数にアクセス
        foreach (FieldInfo field in fields)
        {
            //object value = field.GetValue(instance);
            //Debug.Log(field.Name + ": " + value);
            titleLine += field.Name + ",";
        }



        sw.WriteLine(titleLine);


        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            string line = "";

            line += item.constellation_id + ",";

            foreach (var luckyItemId in item.luckyItemIds)
            {
                line += luckyItemId + ",";
            }
            sw.WriteLine(line);
        }


        AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);
    }

    public class Test
    {
        public string constellation_id;
        public List<int> luckyItemIds = new();
        //  public List<string> luckyColorIds = new();
    }


    [MenuItem("MyTool/Create Fortunes")]
    static async void Start()
    {
        Debug.Log("計算開始");

        await CSVManager.InitializeAsync();
        var Constellations = CSVManager.Constellations;
        var LuckyItemsOrigin = CSVManager.LuckyItems;
        var LuckyColorsOrigin = CSVManager.LuckyColors;

        var rankList = Enumerable.Range(1, 12).ToList();
        var msgNoList = Enumerable.Range(1, 20).ToList();
        var dateTimes = GenerateDateList(365 * 10);

        var dailyFortunesList = new List<List<Fortune>>();


        foreach (var dateTime in dateTimes)
        {
            var ranks = new List<int>(rankList);
            var msgNos = new List<int>(msgNoList);
            var dailyFortunes = new List<Fortune>();
            var beforeDailyFortunes = dailyFortunesList.LastOrDefault();


            foreach (var constellation in Constellations)
            {
                Fortune fortune = new()
                {
                    date_time = dateTime.ToDateKey(),
                    constellation_id = constellation.id,
                    rank = GetBeforeRank(beforeDailyFortunes, constellation.id),
                    lucky_item_id = 0,
                    lucky_color_id = 0,
                    msg_id = msgNos.PopRandom(),
                };
                dailyFortunes.Add(fortune);
            }

            SetLuckies(dailyFortunesList, dailyFortunes, LuckyItemsOrigin, LuckyColorsOrigin);

            SetRanks(dailyFortunes, ranks);

            dailyFortunesList.Add(dailyFortunes);

            if (dateTime.Day == DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
            {
                await UniTask.DelayFrame(1);
                var progress = (float)dailyFortunesList.Count / (float)dateTimes.Count * 100f;
                Debug.Log("計算中 " + Mathf.FloorToInt(progress) + "%");
            }
        }

        var fortunes = new List<Fortune>();
        foreach (var dailyFortunes in dailyFortunesList)
        {
            fortunes.AddRange(dailyFortunes);
        }


        Save("Fortunes", fortunes);

        List<Test> tests = CreateTests(fortunes.ToArray(), Constellations);

        SaveTest("Test", tests);
    }

    static void SetLuckies(List<List<Fortune>> dailyFortunesList, List<Fortune> dailyFortunes, LuckyItem[] LuckyItemsOrigin, LuckyColor[] LuckyColorsOrigin)
    {
        bool retryLuckyItem = true;
        bool retryLuckyColor = true;

        int loopCount = 0;
        while (retryLuckyItem || retryLuckyColor)
        {
            var luckyItems = new List<LuckyItem>(LuckyItemsOrigin);
            var luckyColors = new List<LuckyColor>(LuckyColorsOrigin);
            foreach (var dailyFortune in dailyFortunes)
            {
                var last7DaysFortunes = Last7DaysFortunes(dailyFortunesList, dailyFortune.constellation_id);

                if (retryLuckyItem)
                {
                    var last7DaysLuckyItemIds = last7DaysFortunes
                        .Select(dailyFortune => dailyFortune.lucky_item_id)
                        .ToList();
                    dailyFortune.lucky_item_id = PopRandomWithoutLast7Days_Id(luckyItems, last7DaysLuckyItemIds);
                }
                if (retryLuckyColor)
                {
                    var last7DaysLuckyColorIds = last7DaysFortunes
                        .Select(dailyFortune => dailyFortune.lucky_color_id)
                        .ToList();
                    dailyFortune.lucky_color_id = PopRandomWithoutLast7Days_Id(luckyColors, last7DaysLuckyColorIds);
                }
            }
            retryLuckyItem = dailyFortunes.Select(dailyFortune => dailyFortune.lucky_item_id).Contains(0);
            retryLuckyColor = dailyFortunes.Select(dailyFortune => dailyFortune.lucky_color_id).Contains(0);

            loopCount++;
            if (loopCount > 100)
            {
                Debug.LogError("無限ループ");
                break;
            }
        }
        if (loopCount > 0)
        {
            // Debug.Log("再抽選回数 " + count);
        }
    }


    static int PopRandomWithoutLast7Days_Id<T>(List<T> baseLuckies, List<int> last7DaysBaseLuckyIds) where T : BaseLucky
    {
        T baseLucky = null;
        if (last7DaysBaseLuckyIds.Count == 0)
        {
            baseLucky = baseLuckies.PopRandom();
            if (baseLucky == null) return 0;
            return baseLucky.id;
        }

        baseLucky = baseLuckies.PopRandom(ignore: baseLucky => last7DaysBaseLuckyIds.Contains(baseLucky.id));
        if (baseLucky == null) return 0;
        return baseLucky.id;
    }

    static List<Fortune> Last7DaysFortunes(List<List<Fortune>> dailyFortunesList, string constellationId)
    {
        var last7DaysDailyFortunesList = dailyFortunesList.ReverseList().Take(7).ToList().ReverseList();
        var last7DaysDailyFortunes = last7DaysDailyFortunesList
            .Select(dailyFortunes => dailyFortunes.FirstOrDefault(dailyFortune => dailyFortune.constellation_id == constellationId))
            .Where(dailyFortune => dailyFortune != null).ToList();
        return last7DaysDailyFortunes;
    }

    static void SetRanks(List<Fortune> dailyFortunes, List<int> ranks)
    {
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
            rank = ranks.PopRandom(ignore: rank => rank <= 3);
            // Debug.Log("前が3位いないのとき " + rank);
            return rank;
        }

        if (10 <= beforeRank)
        {
            rank = ranks.PopRandom(ignore: rank => 10 <= rank);
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

    static void Save<T>(string fileName, List<T> list)
    {
        Debug.Log("書き込み開始");

        string path = Application.dataPath + @"/_MyAssets/CSV/Fortunes/" + fileName + ".csv";
        using StreamWriter sw = File.CreateText(path);

        string titleLine = "";


        // クラスの変数を配列で取得
        FieldInfo[] fields = typeof(T).GetFields();

        // 配列を反復処理して各変数にアクセス
        foreach (FieldInfo field in fields)
        {
            //object value = field.GetValue(instance);
            //Debug.Log(field.Name + ": " + value);
            titleLine += field.Name + ",";
        }



        sw.WriteLine(titleLine);


        for (int i = 0; i < list.Count; i++)
        {
            var fortune = list[i];
            string line = "";
            foreach (FieldInfo field in fields)
            {
                Debug.Log(field.FieldType.IsArray);
                // メンバが配列の場合は要素ごとに処理
                if (field.FieldType.IsArray)
                {
                    Array arrayValue = (Array)field.GetValue(fortune);
                    for (int j = 0; j < arrayValue.Length; j++)
                    {
                        line += arrayValue.GetValue(j) + ",";
                    }
                }
                else
                {
                    line += field.GetValue(fortune) + ",";
                }
            }

            sw.WriteLine(line);
        }


        AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);
    }
}
