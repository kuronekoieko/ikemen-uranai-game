using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System.Linq;

public class Initialize : SingletonMonoBehaviour<Initialize>
{
    [SerializeField] ScreenManager screenManager;
    bool IsInitialized;

    public UnityAction OnUpdate = () => { };

    async void Start()
    {
        IsInitialized = false;

        Application.targetFrameRate = 60;

        await CSVManager.Instance.InitializeAsync();
        InitSaveData(CSVManager.Instance);


        screenManager.OnStart();

        ScreenManager.Instance.Get<HomeScreen>().Open();

        IsInitialized = true;

        return;//テスト
        ScreenManager.Instance.Get<LoadingScreen>().Open();

        var asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false;

        // テスト用
        await ScreenManager.Instance.Get<LoadingScreen>().ProgressTimer(5);
        ScreenManager.Instance.Get<LoadingScreen>().Close();

        ScreenManager.Instance.Get<InputProfileScreen>().Open();

        //asyncOperation.allowSceneActivation = true;


    }

    async void InitSaveData(CSVManager cSVManager)
    {
        var defaultSaveData = new SaveData
        {
            characters = CreateDic(cSVManager)
        };
        SaveDataManager.LoadOverWrite(defaultSaveData);

        // SaveDataManager.Load();
        //  SaveDataManager.SaveData.characters = CreateDic(cSVManager);
        SaveDataManager.Save();

        DebugUtils.LogJson(SaveDataManager.SaveData);

        // DebugUtils.LogJson(SaveData.Instance);

        // 差分追加
        // AddSaveDataCharacters(cSVManager);
        // SaveData.Instance.Save();

        await UniTask.DelayFrame(1);

        DebugUtils.LogJson(SaveDataManager.SaveData);
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

    /*
        void AddSaveDataCharacters(CSVManager cSVManager)
        {
            var saveDataCharacters = SaveDataManager.SaveData.characters.ToList();

            foreach (var dataBaseCharacter in cSVManager.Characters)
            {
                bool exist = false;
                foreach (var saveDataCharacter in SaveDataManager.SaveData.characters)
                {
                    Debug.Log(saveDataCharacter.id + " " + dataBaseCharacter.id);
                    exist = saveDataCharacter.id == dataBaseCharacter.id;
                    if (exist) break;
                }
                if (exist) continue;

                var newSaveDataCharacter = new SaveDataObjects.Character()
                {
                    id = dataBaseCharacter.id
                };
                saveDataCharacters.Add(newSaveDataCharacter);

            }

            SaveDataManager.SaveData.characters = saveDataCharacters;
        }
    */

    private void Update()
    {
        if (IsInitialized == false) return;
        OnUpdate.Invoke();
    }


}
