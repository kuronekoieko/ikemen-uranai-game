using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;


namespace MainScene
{

    public class Initialize : SingletonMonoBehaviour<Initialize>
    {
        [SerializeField] ScreenManager screenManager;
        bool IsInitialized;

        public UnityAction OnUpdate = () => { };

        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoad()
        {
            SceneManager.LoadScene(0);
        }

        private async void Start()
        {
            if (IsInitialized) return;
            IsInitialized = false;

            Application.targetFrameRate = 60;
            // https://qiita.com/norimatsu_yusuke/items/5babc03b27a1715bb56c
            // 同時押し無効
            Input.multiTouchEnabled = false;

            Debug.Log("FirebaseAuthenticationManager.Initialize");
            bool success = await FirebaseAuthenticationManager.Initialize();
            // Debug.Log("FirebaseAuthenticationManager.Initialize success:" + success);
            if (success == false)
            {
                Debug.LogError("FirebaseAuthenticationManager.Initialize success:" + success);
                await ShowInitFailed();
                return;
            }


            Debug.Log("FirebaseDatabaseManager.Initialize");
            // FirebaseStorageManager.Initialize();
            //FirebaseDatabaseManager.Initialize();

            Debug.Log("FirebaseRemoteConfigManager.InitializeAsync");
            // FirebaseCloudMessagingManager.Initialize();
            success = await FirebaseRemoteConfigManager.Initialize();
            if (success == false)
            {
                Debug.LogError("FirebaseRemoteConfigManager.Initialize success:" + success);
                await ShowInitFailed();
                return;
            }


            Debug.Log("GoogleCalendarAPI.GetHolidaysAsync");
            //GoogleCalendarAPI.GetHolidaysAsync(DateTime.Now.Year).Forget();
            await GoogleCalendarAPI.GetHolidaysAsync(DateTime.Now.Year);

            bool is_maintenance = FirebaseRemoteConfigManager.GetBool(FirebaseRemoteConfigManager.Key.is_maintenance);
            if (is_maintenance)
            {
                await PopupManager.Instance.GetCommonPopup().ShowAsync(
                        "",
                        "メンテナンス中です\nしばらく時間をおいてお試しください。",
                        "OK"
                    );
                Quit();
                return;
            }

            string latestVersion = FirebaseRemoteConfigManager.GetString(FirebaseRemoteConfigManager.Key.latest_version);

            if (latestVersion.TrimStart().TrimStart() != Application.version.TrimStart().TrimStart())
            {
                await PopupManager.Instance.GetCommonPopup().ShowAsync(
                        "",
                        "最新バージョンがあります。\nアプリをアップデートしてください。",
                        "OK"
                    );
                Application.OpenURL(URLs.APP_STORE_PAGE);

                Quit();
                return;
            }


            await CSVManager.InitializeAsync();

            Debug.Log("SaveDataInitializer.Initialize ");
            success = await SaveDataInitializer.Initialize(CSVManager.Characters, FirebaseAuthenticationManager.User.UserId);

            if (success == false)
            {
                Debug.LogError("SaveDataInitializer.Initialize success:" + success);

                await ShowInitFailed();
                return;
            }


            OnCompleteInit();
        }

        async UniTask ShowInitFailed()
        {
            await PopupManager.Instance.GetCommonPopup().ShowAsync(
                    "",
                    "サーバーへの接続に失敗しました。\nアプリを再起動してください。",
                    "OK"
                );

            Quit();
        }


        async void OnCompleteInit()
        {
            HomeScreen.Instance.OnStart();
            screenManager.OnStart();

            await UniTask.DelayFrame(1);

            ScreenManager.Instance.Get<LoadingScreen>().Open();

            // ローディング画面を開いてから、スプラッシュを閉じる
            if (InitializeScene.Initialize.Instance) InitializeScene.Initialize.Instance.Close();

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
            }

            await ScreenManager.Instance.Get<LoadingScreen>().Close();

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

        void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

    }

}
