using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
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
            FirebaseCloudMessagingManager.Initialize();

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
            var task0 = ScreenManager.Instance.Get<LoadingScreen>().ProgressTimer(1);
            ReturnLocalPushNotification.SetLocalPush();

            if (SaveDataManager.SaveData.BirthDayDT == null)
            {
                await UniTask.WhenAll(task0);

                ScreenManager.Instance.Get<InputProfileScreen>().Open();
            }
            else
            {
                var task1 = DownloadFilesAsync();
                var task2 = NaninovelInitializer.InitializeAsync(SaveDataManager.SaveData.currentCharacterId);
                await UniTask.WhenAll(task0, task1, task2);

                ScreenManager.Instance.Get<HomeScreen>().Open();
            }

            ScreenManager.Instance.Get<LoadingScreen>().Close();


            IsInitialized = true;
        }



        async UniTask DownloadFilesAsync()
        {

            // 初回起動時は、誕生日情報が無いので、Constellationがnullになる
            var todayFortune = FortuneManager.GetFortune(DateTime.Today, SaveDataManager.SaveData.Constellation.id);
            var tomorrowFortune = FortuneManager.GetFortune(DateTime.Today.AddDays(1), SaveDataManager.SaveData.Constellation.id);

            // var task1 = FileDownloader.DownloadFortune(DateTime.Today);
            // var task2 = FileDownloader.DownloadFortune(DateTime.Today.AddDays(1));
            var todayAudioFileName = AssetBundleLoader.GetAudioFileName(SaveDataManager.SaveData.GetCurrentCharacter(), todayFortune);
            var task3 = AssetBundleLoader.LoadAssetAsync<AudioClip>(todayAudioFileName);

            var tomorrowAudioFileName = AssetBundleLoader.GetAudioFileName(SaveDataManager.SaveData.GetCurrentCharacter(), tomorrowFortune);
            var task4 = AssetBundleLoader.LoadAssetAsync<AudioClip>(tomorrowAudioFileName);

            // var test = "Voices/chara0001-rank01-msg01";
            // var task5 = FileDownloader.GetAudioClip(test);

            await UniTask.WhenAll(task3, task4);
        }


        private void Update()
        {
            if (IsInitialized == false) return;
            OnUpdate.Invoke();
        }

    }

}
