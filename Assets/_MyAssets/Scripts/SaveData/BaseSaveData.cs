using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class BaseSaveData<T> where T : new()
{
    readonly string KEY_SAVE_DATA = "save_data";
    public static T Instance
    {
        get
        {
            _instance ??= new();// if (_instance == null) _instance = new();
            return _instance;
        }
    }
    private static T _instance;

    public void Save()
    {
        //ユーザーデータオブジェクトからjson形式のstringを取得
        // string jsonStr = JsonUtility.ToJson(SaveData.Instance);
        string jsonStr = JsonConvert.SerializeObject(Instance, Formatting.Indented);
        SavePlayerPrefs(jsonStr);
    }

    public void SaveOverWrite(T t)
    {
        _instance = t;
        //ユーザーデータオブジェクトからjson形式のstringを取得
        // string jsonStr = JsonUtility.ToJson(SaveData.Instance);
        string jsonStr = JsonConvert.SerializeObject(Instance, Formatting.Indented);
        SavePlayerPrefs(jsonStr);
    }

    public void LoadSaveData()
    {
        string jsonStr = PlayerPrefs.GetString(KEY_SAVE_DATA);
        //Debug.Log(jsonStr);
        if (string.IsNullOrEmpty(jsonStr) == false)
        {
            // jsonが空のときどうなる？→null
            _instance = JsonConvert.DeserializeObject<T>(jsonStr);
        }

        //アプデ対応(配列のサイズを追加するため)
        AddSaveDataInstance();
        //ユーザーデータ保存
        Save();
    }


    void SavePlayerPrefs(string jsonStr)
    {
        //jsonデータをセットする
        PlayerPrefs.SetString(KEY_SAVE_DATA, jsonStr);
        //保存する
        PlayerPrefs.Save();
    }

    void InitSaveDataInstance()
    {
    }

    void AddSaveDataInstance()
    {
    }

    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        LoadSaveData();
    }

}
