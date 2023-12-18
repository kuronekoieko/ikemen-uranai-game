using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

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

    void InitSaveData(CSVManager cSVManager)
    {
        SaveData.Instance.LoadSaveData();

        DebugUtils.LogJson(SaveData.Instance);

        foreach (var character in cSVManager.Characters)
        {
            var saveDataCharacter = new SaveDataObjects.Character()
            {
                id = character.id
            };
            SaveData.Instance.characters.Add(saveDataCharacter);
        }

        SaveData.Instance.Save();

        DebugUtils.LogJson(SaveData.Instance);

    }


    private void Update()
    {
        if (IsInitialized == false) return;
        OnUpdate.Invoke();
    }


}
