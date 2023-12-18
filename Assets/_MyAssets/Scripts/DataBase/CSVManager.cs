using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DataBase;

public class CSVManager : Singleton<CSVManager>
{
    public Character[] characters;
    public bool IsInitialized { get; private set; }

    public async UniTask InitializeAsync()
    {
        IsInitialized = false;
        // パスに拡張子つけない
        TextAsset textAsset = await LoadAsync<TextAsset>("CSV/Characters");
        if (textAsset) characters = CSVSerializer.Deserialize<Character>(textAsset.text);
        IsInitialized = true;
    }

    async UniTask<T> LoadAsync<T>(string path) where T : Object
    {
        var result = await Resources.LoadAsync<T>(path);
        var resource = result as T;
        if (resource == null) Debug.LogError("csv読み込みに失敗しました: " + path);
        return resource;
    }
}

