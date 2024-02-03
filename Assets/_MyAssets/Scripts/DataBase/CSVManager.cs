using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DataBase;
using System.IO;
using System.Linq;


public static class CSVManager
{
    // 各オブジェクト内の変数名はcsvのキーと同じにする
    public static Character[] Characters { get; private set; }
    public static LevelData[] CharacterLevelDatas { get; private set; }
    public static LevelData[] PlayerLevelDatas { get; private set; }
    public static Constellation[] Constellations { get; private set; }
    public static Fortune[] Fortunes { get; private set; }
    public static Hint[] Hints { get; private set; }
    public static FortuneMessage[] FortuneMessages { get; private set; }
    public static LuckyItem[] LuckyItems { get; private set; }
    public static LuckyColor[] LuckyColors { get; private set; }
    public static ChargingProduct[] ChargingProducts { get; private set; }
    public static HomeText[] HomeTexts { get; private set; }
    public static Date[] Dates { get; private set; }
    public static Day[] Days { get; private set; }
    public static DataBase.Time[] Times { get; private set; }

    public static async UniTask InitializeAsync()
    {
        Characters = await DeserializeAsync<Character>("Characters");
        CharacterLevelDatas = await DeserializeAsync<LevelData>("CharacterLevel-Exp");
        PlayerLevelDatas = await DeserializeAsync<LevelData>("PlayerLevel-Exp");
        Constellations = await DeserializeAsync<Constellation>("Constellations");
        Fortunes = await DeserializeAsync<Fortune>("Fortunes");
        Hints = await DeserializeAsync<Hint>("Hint");
        LuckyItems = await DeserializeAsync<LuckyItem>("Fortunes/LuckyItems");
        LuckyColors = await DeserializeAsync<LuckyColor>("Fortunes/LuckyColors");
        ChargingProducts = await DeserializeAsync<ChargingProduct>("ChargingProducts");
        var fortuneMessageDics = await DeserializeAsync_StringDics("FortuneMessages");
        FortuneMessages = ToObjects(fortuneMessageDics).ToArray();
        await InitHomeText();
    }

    static async UniTask InitHomeText()
    {
        HomeTexts = await DeserializeAsync<HomeText>("Home/HomeTexts");
        Dates = await DeserializeAsync<Date>("Home/Dates");
        Days = await DeserializeAsync<Day>("Home/Days");
        Times = await DeserializeAsync<DataBase.Time>("Home/Times");

        foreach (var homeText in HomeTexts)
        {
            homeText.date = Dates.FirstOrDefault(date => date.date_id == homeText.date_id);
            if (homeText.date == null)
            {
                Debug.LogError($"id:{homeText.id} date が存在しません");
            }
            homeText.day = Days.FirstOrDefault(date => date.day_id == homeText.day_id);
            if (homeText.date == null)
            {
                Debug.LogError($"id:{homeText.id} day が存在しません");
            }
            homeText.time = Times.FirstOrDefault(date => date.time_id == homeText.time_id);
            if (homeText.date == null)
            {
                Debug.LogError($"id:{homeText.id} time が存在しません");
            }

            // DebugUtils.LogJson(homeText);
        }

    }

    public static async UniTask<T[]> DeserializeAsync<T>(string fileName)
    {
        // パスに拡張子つけない
        string path = "CSV/" + fileName;
        var result = await Resources.LoadAsync<TextAsset>(path);
        var textAsset = result as TextAsset;
        if (textAsset == null)
        {
            Debug.LogError("csv読み込みに失敗しました: " + path);
            return null;
        }

        var ary = CSVSerializer.Deserialize<T>(textAsset.text);
        if (ary == null)
        {
            Debug.LogError("csvデシリアライズに失敗しました: " + fileName);
            return null;
        }
        return ary;
    }



    static async UniTask<List<Dictionary<string, string>>> DeserializeAsync_StringDics(string fileName)
    {
        // パスに拡張子つけない
        string path = "CSV/" + fileName;
        var result = await Resources.LoadAsync<TextAsset>(path);
        var textAsset = result as TextAsset;
        if (textAsset == null) Debug.LogError("csv読み込みに失敗しました: " + path);

        var stringLists = CSVToStringLists(textAsset.text);
        var stringDics = CSVToStringDics(stringLists);

        //if (ary == null) Debug.LogError("csvデシリアライズに失敗しました: " + fileName);
        return stringDics;
    }


    static FortuneMessage ToFortuneMessage(Dictionary<string, string> stringDic)
    {
        FortuneMessage instance = new();
        int.TryParse(stringDic["character_id"], out instance.character_id);
        int.TryParse(stringDic["rank"], out instance.rank);

        List<string> messages = new();
        string[] keys = stringDic.Keys.ToArray();

        foreach (var key in keys)
        {
            if (key.Contains("msg"))
            {
                messages.Add(stringDic[key]);
            }
        }
        instance.messages = messages.ToArray();
        return instance;
    }


    static List<FortuneMessage> ToObjects(List<Dictionary<string, string>> stringDics)
    {
        List<FortuneMessage> instances = new();

        foreach (var stringDic in stringDics)
        {
            FortuneMessage instance = ToFortuneMessage(stringDic);

            instances.Add(instance);
        }

        return instances;
    }


    static List<Dictionary<string, string>> CSVToStringDics(List<string[]> stringLists)
    {
        List<Dictionary<string, string>> datas = new();
        string[] keys = stringLists[0];

        for (int y = 1; y < stringLists.Count; y++)
        {
            Dictionary<string, string> dic = new();

            for (int x = 0; x < keys.Length; x++)
            {
                dic[keys[x]] = stringLists[y][x];
            }
            datas.Add(dic);
        }

        return datas;
    }

    static List<string[]> CSVToStringLists(string csvText)
    {
        StringReader reader = new(csvText);
        List<string[]> csvDatas = new(); // CSVの中身を入れるリスト;

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        return csvDatas;
    }
}

