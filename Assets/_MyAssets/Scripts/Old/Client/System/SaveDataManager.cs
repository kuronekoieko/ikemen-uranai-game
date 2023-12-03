using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager i;
    string KEY_SAVE_DATA = "SaveData";


    void Awake()
    {
        i = this;
    }

    public void Save()
    {
        //ユーザーデータオブジェクトからjson形式のstringを取得
        string jsonStr = JsonUtility.ToJson(SaveData.i);

        SavePlayerPrefs(jsonStr);
    }

    public void LoadSaveData()
    {
        SaveData.i = new SaveData(0, 2000);
        // SaveData.i.Test();

        //PlayerPrefsに保存済みのユーザーデータのstringを取得
        //第二引数に初回起動時のデータを入れる
        string jsonStr = PlayerPrefs.GetString(KEY_SAVE_DATA);
        //ユーザーデータオブジェクトに読み出したデータを格納
        //※このとき、新しく追加された変数は消されずマージされる
        JsonUtility.FromJsonOverwrite(jsonStr, SaveData.i);
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
