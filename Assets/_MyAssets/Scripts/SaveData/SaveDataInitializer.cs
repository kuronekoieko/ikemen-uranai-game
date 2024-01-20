using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using SaveDataObjects;

public static class SaveDataInitializer
{

    public static async UniTask Initialize(DataBase.Character[] databaseCharacters, string firebaseUserId)
    {
        var defaultSaveData = new SaveData
        {
            characters = CreateCharacters(databaseCharacters),
            firebaseUserId = firebaseUserId,
            userId = UserIdManager.DefaultUserId,
        };

        await SaveDataManager.LoadOverWriteAsync(defaultSaveData);

        SaveData saveData = SaveDataManager.SaveData;

        // セーブデータの新規作成
        if (saveData.userId == defaultSaveData.userId)
        {
            saveData.userId = await UserIdManager.CreateNewUserId();
        }

        string key = DateTime.Today.ToDateKey();
        bool existHistory = saveData.horoscopeHistories.TryGetValue(key, out HoroscopeHistory horoscopeHistory);
        if (existHistory == false)
        {
            saveData.horoscopeHistories[key] = new HoroscopeHistory();
        }

        // 時刻も保存したいので
        // 2024/01/18 23:19:23
        saveData.lastLoginDateTime = DateTime.Now.ToString();

        SaveDataManager.Save();
    }

    static Dictionary<string, Character> CreateCharacters(DataBase.Character[] databaseCharacters)
    {
        var saveDataCharacters = new Dictionary<string, Character>();

        foreach (var dataBaseCharacter in databaseCharacters)
        {
            var newSaveDataCharacter = new Character()
            {
                id = dataBaseCharacter.id
            };
            saveDataCharacters.Add(newSaveDataCharacter.IdToKey(), newSaveDataCharacter);
        }
        return saveDataCharacters;
    }
}
