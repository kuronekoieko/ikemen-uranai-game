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

public class CreateFortunes
{
    static Constellation[] Constellations;
    static LuckyItem[] LuckyItems;
    static LuckyColor[] LuckyColors;

    [MenuItem("MyTool/Create Fortunes")]
    static async void Start()
    {
        Constellations = await DeserializeAsync<Constellation>("Constellation");
        LuckyItems = await DeserializeAsync<LuckyItem>("Fortunes/LuckyItems");
        LuckyColors = await DeserializeAsync<LuckyColor>("Fortunes/LuckyColors");

        var dateTimes = GenerateDateList(60);

        foreach (var dateTime in dateTimes)
        {
            List<Fortune> fortunes = new();
            var luckyItems = LuckyItems.ToList();
            var luckyColors = LuckyColors.ToList();
            var ranks = Enumerable.Range(1, 12).ToList();

            foreach (var constellation in Constellations)
            {
                Fortune fortune = new()
                {
                    constellation_id = constellation.id,
                    rank = ranks.PopRandom(),
                    item = luckyItems.PopRandom().name,
                    color = luckyColors.PopRandom().name,
                };
                fortunes.Add(fortune);
            }

            Save(dateTime.ToString("yyyy-MM-dd"), fortunes);
            await UniTask.DelayFrame(1);
        }


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
        string path = Application.dataPath + @"/_MyAssets/Resources/CSV/Fortunes/Daily/" + fileName + ".csv";
        using StreamWriter sw = File.CreateText(path);

        string titleLine = "constellation_id" + "," + "rank" + "," + "item" + "," + "color";
        sw.WriteLine(titleLine);

        foreach (var fortune in fortunes)
        {
            string line = fortune.constellation_id + "," + fortune.rank + "," + fortune.item + "," + fortune.color;
            sw.WriteLine(line);
        }
        AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);
    }

    static async UniTask<T[]> DeserializeAsync<T>(string fileName)
    {
        // パスに拡張子つけない
        string path = "CSV/" + fileName;
        var result = await Resources.LoadAsync<TextAsset>(path);
        var textAsset = result as TextAsset;
        if (textAsset == null) Debug.LogError("csv読み込みに失敗しました: " + path);

        var ary = CSVSerializer.Deserialize<T>(textAsset.text);
        if (ary == null) Debug.LogError("csvデシリアライズに失敗しました: " + fileName);
        return ary;
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
