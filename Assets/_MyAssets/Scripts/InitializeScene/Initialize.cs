using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class Initialize : MonoBehaviour
{
    [SerializeField] ScreenManager screenManager;
    async void Start()
    {
        Application.targetFrameRate = 60;
        await CSVManager.Instance.InitializeAsync();

        screenManager.OnStart();

        ScreenManager.Instance.Get<HomeScreen>().Open();
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




}
