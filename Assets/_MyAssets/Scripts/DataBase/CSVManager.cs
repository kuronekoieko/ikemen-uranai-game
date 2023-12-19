using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DataBase;

public class CSVManager : Singleton<CSVManager>
{
    // 各オブジェクト内の変数名はcsvのキーと同じにする
    public Character[] Characters { get; private set; }
    public LevelData[] LevelDatas { get; private set; }
    public Constellation[] Constellations { get; private set; }
    public Fortune[] Fortunes { get; private set; }


    public async UniTask InitializeAsync()
    {
        Characters = await DeserializeAsync<Character>("Characters");
        LevelDatas = await DeserializeAsync<LevelData>("Level-Exp");
        Constellations = await DeserializeAsync<Constellation>("Constellation");
        Fortunes = await DeserializeAsync<Fortune>("Fortune");
    }

    async UniTask<T[]> DeserializeAsync<T>(string fileName)
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

