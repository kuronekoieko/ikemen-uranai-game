using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using SaveDataObjects;

public static class SaveDataInitializer
{

    public static async UniTask<bool> Initialize(DataBase.Character[] databaseCharacters, string firebaseUserId)
    {
        var defaultSaveData = new SaveData
        {
            characters = CreateCharacters(databaseCharacters),
            firebaseUserId = firebaseUserId,
            userId = UserIdManager.DefaultUserId,
        };
        bool success = await SaveDataManager.LoadOverWriteAsync(defaultSaveData);
        // ネットワークエラーの場合
        if (success == false)
        {
            return false;
        }

        SaveData saveData = SaveDataManager.SaveData;

        // セーブデータのチェック＆新規作成
        saveData.userId = await UserIdManager.ValidateUserId(saveData.userId);
        // ネットワークエラーの場合
        if (saveData.userId == null) return false;

        string key = DateTime.Today.ToDateKey();
        bool existHistory = saveData.horoscopeHistories.TryGetValue(key, out HoroscopeHistory horoscopeHistory);
        if (existHistory == false)
        {
            saveData.horoscopeHistories[key] = new HoroscopeHistory();
        }

        // 時刻も保存したいので
        // 2024/01/18 23:19:23
        saveData.lastLoginDateTime = DateTime.Now.ToString();

        success = await SaveDataManager.SaveAsync();
        // ネットワークエラーの場合
        if (success == false)
        {
            return false;
        }
        return true;
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
