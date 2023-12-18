using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveDataManager
{
    readonly static string KEY_SAVE_DATA = "save_data";
    public static SaveData SaveData => _SaveData;
    private static SaveData _SaveData;

    public static void Save()
    {
        //ユーザーデータオブジェクトからjson形式のstringを取得
        // string jsonStr = JsonUtility.ToJson(SaveData.Instance);
        string jsonStr = JsonConvert.SerializeObject(SaveData, Formatting.Indented);
        SavePlayerPrefs(jsonStr);
    }


    public static void Replace(SaveData t)
    {
        _SaveData = t;
        Save();
    }

    public static void Load()
    {
        string jsonStr = PlayerPrefs.GetString(KEY_SAVE_DATA);
        //Debug.Log(jsonStr);
        if (string.IsNullOrEmpty(jsonStr) == false)
        {
            // jsonが空のときどうなる？→null
            _SaveData = JsonConvert.DeserializeObject<SaveData>(jsonStr);
        }
        else
        {
            _SaveData = new();
        }

        //ユーザーデータ保存
        Save();
    }

    public static void LoadOverWrite(SaveData defaultSaveData)
    {
        _SaveData = defaultSaveData;

        string jsonStr = PlayerPrefs.GetString(KEY_SAVE_DATA);
        //Debug.Log(jsonStr);
        if (string.IsNullOrEmpty(jsonStr) == false)
        {
            // jsonにnullがあると、nullで上書きされるので注意(特にリストやdic)
            JsonConvert.PopulateObject(jsonStr, _SaveData);
        }

        //ユーザーデータ保存
        Save();
    }


    static void SavePlayerPrefs(string jsonStr)
    {
        //jsonデータをセットする
        PlayerPrefs.SetString(KEY_SAVE_DATA, jsonStr);
        //保存する
        PlayerPrefs.Save();
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
        Load();
    }
}
