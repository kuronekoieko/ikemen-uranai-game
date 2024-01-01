using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DataBase;
using System.IO;
using System.Linq;


public class CSVManager : Singleton<CSVManager>
{
    // 各オブジェクト内の変数名はcsvのキーと同じにする
    public Character[] Characters { get; private set; }
    public LevelData[] CharacterLevelDatas { get; private set; }
    public LevelData[] PlayerLevelDatas { get; private set; }
    public Constellation[] Constellations { get; private set; }
    public Fortune[] Fortunes { get; private set; }
    public Hint[] Hints { get; private set; }
    public FortuneMessage[] FortuneMessages { get; private set; }


    public async UniTask InitializeAsync()
    {
        Characters = await DeserializeAsync<Character>("Characters");
        CharacterLevelDatas = await DeserializeAsync<LevelData>("CharacterLevel-Exp");
        PlayerLevelDatas = await DeserializeAsync<LevelData>("PlayerLevel-Exp");
        Constellations = await DeserializeAsync<Constellation>("Constellation");
        Fortunes = await DeserializeAsync<Fortune>("Fortunes");
        Hints = await DeserializeAsync<Hint>("Hint");

        var fortuneMessageDics = await DeserializeAsync_StringDics("FortuneMessages");
        FortuneMessages = ToObjects(fortuneMessageDics).ToArray();
    }

    public async UniTask<T[]> DeserializeAsync<T>(string fileName)
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



    async UniTask<List<Dictionary<string, string>>> DeserializeAsync_StringDics(string fileName)
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


    FortuneMessage ToFortuneMessage(Dictionary<string, string> stringDic)
    {
        FortuneMessage instance = new()
        {
            character_id = stringDic["character_id"],
        };
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


    List<FortuneMessage> ToObjects(List<Dictionary<string, string>> stringDics)
    {
        List<FortuneMessage> instances = new();

        foreach (var stringDic in stringDics)
        {
            FortuneMessage instance = ToFortuneMessage(stringDic);

            instances.Add(instance);
        }

        return instances;
    }


    List<Dictionary<string, string>> CSVToStringDics(List<string[]> stringLists)
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

    List<string[]> CSVToStringLists(string csvText)
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

