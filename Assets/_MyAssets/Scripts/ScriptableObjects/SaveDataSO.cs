using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System.IO;

[CreateAssetMenu(menuName = "MyGame/Create " + nameof(SaveDataSO), fileName = nameof(SaveDataSO))]
public class SaveDataSO : ScriptableObject
{
    [SerializeField] SaveData saveData;


    [Button]
    public void Pull()
    {
        SaveDataManager.Load();
        saveData = SaveDataManager.SaveData;
        SaveJson(saveData);
        Debug.Log("セーブデータ取得");
    }

    [Button]
    public void Send()
    {
        SaveDataManager.Replace(saveData);
        Debug.Log("セーブデータ更新");
    }

    [Button]
    public void Clear()
    {
        SaveDataManager.Clear();
        saveData = SaveDataManager.SaveData;
        SaveJson(saveData);
        Debug.Log("セーブデータ削除");
    }

    [Button]
    public void SendJson()
    {
        var jsonTA = Resources.Load<TextAsset>("Json/SaveData/SaveData");
        Debug.Log(jsonTA);
        if (jsonTA == null) return;
        saveData = JsonConvert.DeserializeObject<SaveData>(jsonTA.text);
        Send();
    }

    public void SaveJson(object obj)
    {
        string path = Application.dataPath + @"/_MyAssets/Resources/Json/SaveData/SaveData.json";
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        using StreamWriter sw = File.CreateText(path);
        sw.WriteLine(json);
    }
   
}