using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

public static class SaveDataManager
{
    readonly static string KEY_SAVE_DATA = "save_data";
    public static SaveData SaveData => _SaveData;
    private static SaveData _SaveData;

    public static async void Save()
    {
        //ユーザーデータオブジェクトからjson形式のstringを取得
        string jsonStr = JsonConvert.SerializeObject(SaveData, Formatting.Indented);
        // SavePlayerPrefs(jsonStr);
        await FirebaseDatabaseManager.Instance.SendSaveData(SaveData);
        Debug.Log(jsonStr);
    }


    public static void Replace(SaveData t)
    {
        _SaveData = t;
        Save();
    }

    public static async UniTask Load()
    {
        /*
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
        */

        _SaveData = await FirebaseDatabaseManager.Instance.GetUserData(_SaveData.uid);
        // _SaveData ??= defaultSaveData;


        //ユーザーデータ保存
        Save();
    }

    public static async UniTask LoadOverWriteAsync(SaveData defaultSaveData)
    {


        /*
         _SaveData = defaultSaveData;
                string jsonStr = PlayerPrefs.GetString(KEY_SAVE_DATA);
                //Debug.Log(jsonStr);
                if (string.IsNullOrEmpty(jsonStr) == false)
                {
                    // jsonにnullがあると、nullで上書きされるので注意(特にリストやdic)
                    JsonConvert.PopulateObject(jsonStr, _SaveData);
                }
                */
        _SaveData = await FirebaseDatabaseManager.Instance.GetUserData(defaultSaveData.uid);
        _SaveData ??= defaultSaveData;

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

        // TODO: サーバーも消す
        //await Load();
    }
}
