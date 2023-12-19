using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DataBase;

public class CSVManager : Singleton<CSVManager>
{
    public Character[] Characters { get; private set; }
    public LevelData[] LevelDatas { get; private set; }
    public Constellation[] Constellations { get; private set; }


    public async UniTask InitializeAsync()
    {
        Characters = await Deserialize<Character>("Characters");
        LevelDatas = await Deserialize<LevelData>("Level-Exp");
        Constellations = await Deserialize<Constellation>("Constellation");

        foreach (var item in Constellations)
        {
            DebugUtils.LogJson(item);
        }
    }

    async UniTask<T[]> Deserialize<T>(string fileName)
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
}

