using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


public static class SaveDataInitializer
{

    public static async UniTask Initialize(CSVManager cSVManager)
    {
        var defaultSaveData = new SaveData
        {
            characters = CreateCharacters(cSVManager)
        };

        await SaveDataManager.LoadOverWriteAsync(defaultSaveData);
        // DebugUtils.LogJson(SaveDataManager.SaveData);
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
}
