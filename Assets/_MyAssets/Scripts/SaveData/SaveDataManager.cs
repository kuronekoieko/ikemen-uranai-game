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

    public static async UniTask SaveAsync()
    {
        //ユーザーデータオブジェクトからjson形式のstringを取得
        string jsonStr = JsonConvert.SerializeObject(SaveData, Formatting.Indented);
        // SavePlayerPrefs(jsonStr);
        await FirebaseDatabaseManager.SendSaveData(SaveData);
        Debug.Log(jsonStr);
    }


    public static async void Replace(SaveData t)
    {
        _SaveData = t;
        await SaveAsync();
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

        _SaveData = await FirebaseDatabaseManager.GetUserData(_SaveData.firebaseUserId);

        // _SaveData ??= defaultSaveData;


        //ユーザーデータ保存
        await SaveAsync();
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
        _SaveData = await FirebaseDatabaseManager.GetUserData(defaultSaveData.firebaseUserId);
        _SaveData ??= defaultSaveData;

        if (_SaveData == null)
        {
            _SaveData = defaultSaveData;
            //ユーザーデータ保存
            await SaveAsync();
        }
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
