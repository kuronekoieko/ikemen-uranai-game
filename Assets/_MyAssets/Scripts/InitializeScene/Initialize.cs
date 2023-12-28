using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System.Linq;
using System;

public class Initialize : SingletonMonoBehaviour<Initialize>
{
    [SerializeField] ScreenManager screenManager;
    bool IsInitialized;

    public UnityAction OnUpdate = () => { };

    async void Start()
    {
        IsInitialized = false;

        Application.targetFrameRate = 60;
        // await GoogleCloudStorage.DownloadAudioFile();
        FirebaseStorageManager.Instance.Initialize();

        await CSVManager.Instance.InitializeAsync();
        SaveDataInitializer.Initialize(CSVManager.Instance);

        screenManager.OnStart();

        ScreenManager.Instance.Get<LoadingScreen>().Open();

        // var asyncOperation = SceneManager.LoadSceneAsync("Main");
        // asyncOperation.allowSceneActivation = false;

        // テスト用
        await ScreenManager.Instance.Get<LoadingScreen>().ProgressTimer(1);
        ScreenManager.Instance.Get<LoadingScreen>().Close();

        if (SaveDataManager.SaveData.BirthDayDT == null)
        {
            ScreenManager.Instance.Get<InputProfileScreen>().Open();
        }
        else
        {
            ScreenManager.Instance.Get<HomeScreen>().Open();
        }

        //asyncOperation.allowSceneActivation = true;

        IsInitialized = true;
    }

    private void Update()
    {
        if (IsInitialized == false) return;
        OnUpdate.Invoke();
    }


}
