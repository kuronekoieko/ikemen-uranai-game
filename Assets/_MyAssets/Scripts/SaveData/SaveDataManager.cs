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

    public static async UniTask<bool> SaveAsync()
    {
        //ユーザーデータオブジェクトからjson形式のstringを取得
        string jsonStr = JsonConvert.SerializeObject(SaveData, Formatting.Indented);
        // SavePlayerPrefs(jsonStr);
        bool success = await FirebaseDatabaseManager.SendSaveData(SaveData);
        Debug.Log(jsonStr);
        return success;
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

        (bool success, SaveData saveData) = await FirebaseDatabaseManager.GetUserData(_SaveData.firebaseUserId);

        // _SaveData ??= defaultSaveData;


        //ユーザーデータ保存
        await SaveAsync();
    }

    public static async UniTask<bool> LoadOverWriteAsync(SaveData defaultSaveData)
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
        (bool success, SaveData saveData) = await FirebaseDatabaseManager.GetUserData(defaultSaveData.firebaseUserId);
        if (success == false)
        {
            return success;
        }


        if (saveData == null)
        {
            _SaveData = defaultSaveData;
            // 初回起動時に、二回セーブすることになるので不要
            // 分岐せず一箇所で
            //await SaveAsync();
        }
        else
        {
            _SaveData = saveData;
        }
        success = true;
        return success;

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
