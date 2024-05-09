using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System;

public class Initialize : SingletonMonoBehaviour<Initialize>
{
    [SerializeField] SplashCanvas splashCanvas;
    bool IsInitialized;
    public UnityAction OnUpdate = () => { };

    PopupManager PopupManager => PopupManager.Instance;
    ScreenManager ScreenManager => ScreenManager.Instance;

    async void Start()
    {
        if (IsInitialized) return;
        IsInitialized = false;

        Application.targetFrameRate = 60;
        // https://qiita.com/norimatsu_yusuke/items/5babc03b27a1715bb56c
        // 同時押し無効
        Input.multiTouchEnabled = false;

        await splashCanvas.Open();

        await PopupManager.OnStart();

        await CSVManager.InitializePopupTexts();

        bool isCanceled = await OnlineCheckPopupManager.WaitUntilOnline();
        if (isCanceled)
        {
            Quit();
            return;
        }
        /*

        (bool success_0, bool success_1) = await UniTask.WhenAll(
            FirebaseRemoteConfigManager.Initialize(),
            FirebaseAuthenticationManager.Initialize());*/

        isCanceled = await ShowInitFailed(async () =>
        {
            bool success = await FirebaseRemoteConfigManager.Initialize();
            if (success == false) Debug.LogError("FirebaseRemoteConfigManager.Initialize failed");
            return success;
        });
        if (isCanceled)
        {
            Quit();
            return;
        }

        isCanceled = await ShowInitFailed(async () =>
        {
            bool success = await FirebaseAuthenticationManager.Initialize();
            if (success == false) Debug.LogError("FirebaseAuthenticationManager.Initialize failed");
            return success;
        });
        if (isCanceled)
        {
            Quit();
            return;
        }



        bool is_maintenance = FirebaseRemoteConfigManager.GetBool(FirebaseRemoteConfigManager.Key.is_maintenance);
        if (is_maintenance)
        {
            string maintenance_message = FirebaseRemoteConfigManager.GetString(FirebaseRemoteConfigManager.Key.maintenance_message);

            var popupText = CSVManager.GetPopupText(2);
            popupText.text = maintenance_message;

            await PopupManager.GetCommonPopup().ShowAsync(popupText);

            string url = FirebaseRemoteConfigManager.GetString(FirebaseRemoteConfigManager.Key.url_x);
            Application.OpenURL(url);

            Quit();
            return;
        }

        string latestVersion = FirebaseRemoteConfigManager.GetString(FirebaseRemoteConfigManager.Key.latest_version);
        bool needUpdate = latestVersion.TrimStart().TrimStart() != Application.version.TrimStart().TrimStart();
        if (needUpdate)
        {
            await PopupManager.GetCommonPopup().ShowAsync(3);
            string url = FirebaseRemoteConfigManager.GetString(FirebaseRemoteConfigManager.Key.url_app_store_page);
            Application.OpenURL(url);

            Quit();
            return;
        }


        await CSVManager.InitializeAsync();


        isCanceled = await ShowInitFailed(() =>
        {
            // Debug.LogError("SaveDataInitializer.Initialize success:" + success);
            return SaveDataInitializer.Initialize(CSVManager.Characters, FirebaseAuthenticationManager.User.UserId);
        });
        if (isCanceled)
        {
            Quit();
            return;
        }


        OnCompleteInit();
    }

    async UniTask<bool> ShowInitFailed(Func<UniTask<bool>> uniTask)
    {
        bool success = await uniTask();
        bool isCanceled = false;

        while (success == false)
        {
            bool isRetry = await PopupManager.GetCommonPopup().ShowAsync(4);
            if (isRetry == false)
            {
                isCanceled = true;
                return isCanceled;
            }
            await UniTaskUtils.DelaySecond(0.5f);
            success = await uniTask();
        }


        return isCanceled;
    }


    async void OnCompleteInit()
    {
        // この書き方だと、この行の時点で実行がはじまってしまう
        // UniTask googleCalenderTask = GoogleCalendarAPI.GetHolidaysAsync(DateTime.Now.Year);

        await ScreenManager.OnStart();

        ScreenManager.Get<LoadingScreen>().Open();

        // ローディング画面を開いてから、スプラッシュを閉じる
        await splashCanvas.Close();

        LocalPushNotificationManager.SetLocalPush();

        if (SaveDataManager.SaveData.BirthDayDT == null)
        {
            await UniTask.WhenAll(
                ScreenManager.Get<LoadingScreen>().ProgressTimer(1),
                NaninovelManager.InitializeAsync(SaveDataManager.SaveData.currentCharacterId),
                GoogleCalendarAPI.GetHolidaysAsync(DateTime.Now.Year),
                SaveDataManager.SaveAsync(),
                AudioManager.Instance.Initialize());

            ScreenManager.Get<InputProfileScreen>().Open();
        }
        else
        {
            await UniTask.WhenAll(
                ScreenManager.Get<LoadingScreen>().ProgressTimer(1),
                NaninovelManager.InitializeAsync(SaveDataManager.SaveData.currentCharacterId),
                GoogleCalendarAPI.GetHolidaysAsync(DateTime.Now.Year),
                SaveDataManager.SaveAsync(),
                AudioManager.Instance.Initialize(),
                DownloadFilesAsync());

            ScreenManager.Get<HomeScreen>().Open();

        }

        await ScreenManager.Get<LoadingScreen>().Close();

        IsInitialized = true;
    }



    async UniTask DownloadFilesAsync()
    {

        // 初回起動時は、誕生日情報が無いので、Constellationがnullになる
        var todayFortune = FortuneManager.GetFortune(DateTime.Today, SaveDataManager.SaveData.Constellation.id);
        var tomorrowFortune = FortuneManager.GetFortune(DateTime.Today.AddDays(1), SaveDataManager.SaveData.Constellation.id);

        // var task1 = FileDownloader.DownloadFortune(DateTime.Today);
        // var task2 = FileDownloader.DownloadFortune(DateTime.Today.AddDays(1));
        var todayAudioFileName = AddressablesLoader.GetAudioFileName(SaveDataManager.SaveData.GetCurrentCharacter(), todayFortune);
        var task3 = AddressablesLoader.LoadAsync<AudioClip>(todayAudioFileName);

        var tomorrowAudioFileName = AddressablesLoader.GetAudioFileName(SaveDataManager.SaveData.GetCurrentCharacter(), tomorrowFortune);
        var task4 = AddressablesLoader.LoadAsync<AudioClip>(tomorrowAudioFileName);

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
        Debug.Log("Quit");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

}
