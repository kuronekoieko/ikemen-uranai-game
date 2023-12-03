using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class Initialize : MonoBehaviour
{
    async void Start()
    {
        Application.targetFrameRate = 60;

        LoadingScreen.Instance.OnStart();
        LoadingScreen.Instance.Open();
        InputProfileScreen.Instance.OnStart();
        InputProfileScreen.Instance.Open();

        var asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false;

        // テスト用
        await LoadingScreen.Instance.ProgressTimer(5);


        //asyncOperation.allowSceneActivation = true;


    }




}
