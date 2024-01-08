using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;

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
        if (SaveDataManager.SaveData.userNumber == 0)
        {
            SaveDataManager.SaveData.userNumber = await CreateUserNumberAsync();
            SaveDataManager.SaveData.displayUserId = SaveDataManager.SaveData.userNumber.ToString("D8");
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
            saveDataCharacters.Add(newSaveDataCharacter.IdToKey(), newSaveDataCharacter);
        }
        return saveDataCharacters;
    }
    static async Task<int> CreateUserNumberAsync()
    {
        var users = await FirebaseDatabaseManager.Instance.GetUsers();
        var maxUserNumber = users.Select(user => user.userNumber).OrderByDescending(userNumber => userNumber).FirstOrDefault();
        return maxUserNumber + 1;
    }

    static async UniTask<string> CreateDisplayUserId(string firebaseUserId)
    {
        var users = await FirebaseDatabaseManager.Instance.GetUsersDic();
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
