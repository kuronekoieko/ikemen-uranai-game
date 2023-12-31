using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System;

public static class SaveDataInitializer
{

    public static async UniTask Initialize(CSVManager cSVManager, string firebaseUserId)
    {
        var defaultSaveData = new SaveData
        {
            characters = CreateCharacters(cSVManager),
            firebaseUserId = firebaseUserId,
        };

        await SaveDataManager.LoadOverWriteAsync(defaultSaveData);
        // DebugUtils.LogJson(SaveDataManager.SaveData);
        if (string.IsNullOrEmpty(SaveDataManager.SaveData.displayUserId))
        {
            SaveDataManager.SaveData.displayUserId = await CreateDisplayUserId(firebaseUserId);
            SaveDataManager.Save();
        }

    }

    static Dictionary<string, SaveDataObjects.Character> CreateCharacters(CSVManager cSVManager)
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

    static async UniTask<string> CreateDisplayUserId(string firebaseUserId)
    {
        var users = await FirebaseDatabaseManager.Instance.GetUsers();
        // TODO: ユーザーが削除されると、被る可能性がある
        int index = GetKeyIndex(users, firebaseUserId);
        return index.ToString("D8");
    }

    static int GetKeyIndex(Dictionary<string, SaveData> dictionary, string key)
    {
        int index = 0;
        foreach (var kvp in dictionary)
        {
            if (EqualityComparer<string>.Default.Equals(kvp.Key, key))
            {
                return index;
            }
            index++;
        }
        return -1; // キーが見つからない場合
    }
}
