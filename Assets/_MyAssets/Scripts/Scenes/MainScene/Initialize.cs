using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System.Linq;
using System;

namespace MainScene
{

    public class Initialize : SingletonMonoBehaviour<Initialize>
    {
        [SerializeField] ScreenManager screenManager;
        bool IsInitialized;

        public UnityAction OnUpdate = () => { };

        private async void Start()
        {
            if (IsInitialized) return;
            IsInitialized = false;

            Application.targetFrameRate = 60;
            await FirebaseAuthenticationManager.Instance.Initialize();
            FirebaseStorageManager.Instance.Initialize();
            FirebaseDatabaseManager.Instance.Initialize();

            await CSVManager.Instance.InitializeAsync();
            await SaveDataInitializer.Initialize(
                CSVManager.Instance,
                FirebaseAuthenticationManager.Instance.User.UserId);

            screenManager.OnStart();

            ScreenManager.Instance.Get<LoadingScreen>().Open();

            // ローディング画面を開いてから、スプラッシュを閉じる
            if (InitializeScene.Initialize.Instance) InitializeScene.Initialize.Instance.Close();
            CompleteInit();
        }


        async void CompleteInit()
        {
            var task1 = FileDownloader.DownloadFortune(DateTime.Today);
            var task2 = FileDownloader.DownloadFortune(DateTime.Today.AddDays(1));
            var task3 = FileDownloader.DownloadAudioClip("test-001.wav");
            var task4 = ScreenManager.Instance.Get<LoadingScreen>().ProgressTimer(1);
            await UniTask.WhenAll(task1, task2, task3, task4);


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

}
