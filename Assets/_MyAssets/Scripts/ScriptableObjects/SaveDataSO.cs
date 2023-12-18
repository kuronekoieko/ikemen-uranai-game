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





    [Button]
    public async void Test()
    {

        await CSVManager.Instance.InitializeAsync();
        InitSaveData(CSVManager.Instance);
    }


    void InitSaveData(CSVManager cSVManager)
    {
        var defaultSaveData = new SaveData
        {
            characters = CreateDic(cSVManager)
        };
        DebugUtils.LogJson(defaultSaveData);

        SaveDataManager.Load();
        DebugUtils.LogJson(SaveDataManager.SaveData);


        SaveDataManager.LoadOverWrite(defaultSaveData);

        // SaveDataManager.Load();
        //  SaveDataManager.SaveData.characters = CreateDic(cSVManager);
        SaveDataManager.Save();

        DebugUtils.LogJson(SaveDataManager.SaveData);

        // DebugUtils.LogJson(SaveData.Instance);

        // 差分追加
        // AddSaveDataCharacters(cSVManager);
        // SaveData.Instance.Save();

        // await UniTask.DelayFrame(1);

        // DebugUtils.LogJson(SaveDataManager.SaveData);
    }

    Dictionary<string, SaveDataObjects.Character> CreateDic(CSVManager cSVManager)
    {
        var saveDataCharacters = new Dictionary<string, SaveDataObjects.Character>();

        foreach (var dataBaseCharacter in cSVManager.Characters)
        {
            var newSaveDataCharacter = new SaveDataObjects.Character()
            {
                id = dataBaseCharacter.id
            };
            saveDataCharacters.Add(newSaveDataCharacter.id, newSaveDataCharacter);

        }
        return saveDataCharacters;
    }

    List<SaveDataObjects.Character> CreateDataCharacters(CSVManager cSVManager)
    {
        var saveDataCharacters = new List<SaveDataObjects.Character>();

        foreach (var dataBaseCharacter in cSVManager.Characters)
        {
            var newSaveDataCharacter = new SaveDataObjects.Character()
            {
                id = dataBaseCharacter.id
            };
            saveDataCharacters.Add(newSaveDataCharacter);

        }
        return saveDataCharacters;
    }
}