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
            // https://qiita.com/norimatsu_yusuke/items/5babc03b27a1715bb56c
            // 同時押し無効
            Input.multiTouchEnabled = false;

            await FirebaseAuthenticationManager.Initialize();
            FirebaseStorageManager.Initialize();
            FirebaseDatabaseManager.Initialize();
            FirebaseCloudMessagingManager.Initialize();
            FirebaseRemoteConfigManager.InitializeAsync().Forget();
            GoogleCalendarAPI.GetHolidaysAsync(DateTime.Now.Year).Forget();


            bool is_maintenance = await FirebaseRemoteConfigManager.GetBool(FirebaseRemoteConfigManager.Key.is_maintenance);
            if (is_maintenance)
            {
                await PopupManager.Instance.GetCommonPopup().ShowAsync(
                        "",
                        "メンテナンス中です\nしばらく時間をおいてお試しください。",
                        "OK"
                    );
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                return;
            }


            string latestVersion = await FirebaseRemoteConfigManager.GetString(FirebaseRemoteConfigManager.Key.latest_version);

            if (latestVersion.TrimStart().TrimStart() != Application.version.TrimStart().TrimStart())
            {
                await PopupManager.Instance.GetCommonPopup().ShowAsync(
                        "",
                        "最新バージョンがあります。\nアプリをアップデートしてください。",
                        "OK"
                    );
                Application.OpenURL("https://apps.apple.com/jp/charts/iphone");

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                return;
            }




            await CSVManager.InitializeAsync();
            await SaveDataInitializer.Initialize(CSVManager.Characters, FirebaseAuthenticationManager.User.UserId);

            screenManager.OnStart();

            ScreenManager.Instance.Get<LoadingScreen>().Open();

            // ローディング画面を開いてから、スプラッシュを閉じる
            if (InitializeScene.Initialize.Instance) InitializeScene.Initialize.Instance.Close();
            CompleteInit();
        }


        async void CompleteInit()
        {
            var loadingScreenTask = ScreenManager.Instance.Get<LoadingScreen>().ProgressTimer(1);
            ReturnLocalPushNotification.SetLocalPush();
            var naninovelTask = NaninovelManager.InitializeAsync(SaveDataManager.SaveData.currentCharacterId);

            if (SaveDataManager.SaveData.BirthDayDT == null)
            {
                await UniTask.WhenAll(loadingScreenTask, naninovelTask);

                ScreenManager.Instance.Get<InputProfileScreen>().Open();
            }
            else
            {
                var downLoadTask = DownloadFilesAsync();
                await UniTask.WhenAll(loadingScreenTask, downLoadTask, naninovelTask);

                ScreenManager.Instance.Get<HomeScreen>().Open();
            }

            ScreenManager.Instance.Get<LoadingScreen>().Close();

            //await NaninovelInitializer.PlayAsync("Home/chara001-text001");
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
