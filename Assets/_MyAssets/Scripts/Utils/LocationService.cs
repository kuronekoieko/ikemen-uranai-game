using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocationService
{
    // https://docs.unity3d.com/ja/current/ScriptReference/LocationService.Start.html

    // Unity iOS/Androidで位置情報を取得する
    // https://qiita.com/hirano/items/dde92f4ed76fb377746e
    public static async void Start()
    {
        // unityエディタ上では通る
        // iosだと、「許可しない」にしても通らない
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("LocationService: " + "not enabled by user");
            OpenSettings.Open();
            return;
        }


        // 仕様:
        // ボタンを押したらダイアログを出す
        // ダイアログが出ない場合、設定画面に移動する
        // ダイアログが出たかどうか判定する方法が無い

        // Starts the location service.
        // 未設定or「許可しない」or「一度だけ許可」の場合にダイアログ出る
        // ダイアログ「一度だけ許可」Running、「アプリの使用中は許可」Running、「許可しない」Failed→もう一度押す→ダイアログ出ない
        // 設定画面「しない」→もう一度押す→ダイアログ出ない:Failed
        // 設定画面「次回または共有時に確認」→もう一度押す→ダイアログ出る:Initializing
        // 設定画面「このアプリを使用中」→もう一度押す→ダイアログ出ない:Running
        Input.location.Start();

        // Waits until the location service initializes
        UniTask initializingTask = UniTask.WaitWhile(() => Input.location.status == LocationServiceStatus.Initializing);
        UniTask timeOutTask = UniTask.Delay(1000 * 20);
        var result = await UniTask.WhenAny(initializingTask, timeOutTask);

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (result == 1)
        {
            Debug.Log("LocationService: " + "Timed out");
            return;
        }

        OpenSettings.Open();


        Debug.Log("LocationService: " + Input.location.status);

        switch (Input.location.status)
        {
            case LocationServiceStatus.Stopped:
                // Input.location.Stop();すると通る
                break;
            case LocationServiceStatus.Running:
                // 許可するとここ通る
                break;
            case LocationServiceStatus.Failed:
                // 「許可しない」押すと通る
                break;
            default:
                break;
        }
        Input.location.Stop();
        // Debug.Log("LocationService: " + Input.location.status);
    }
}
