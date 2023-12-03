using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// アプリがバックグラウンドにいるかを監視するクラス
/// </summary>
public class ApplicationBackgroundObserver : MonoBehaviour
{

    //インスタンス
    public static ApplicationBackgroundObserver Instance { get; private set; }

    //バックグラウンドに行っているか
    private bool _isBackground = false;

    //アプリがバックグラウンドにいるかのステータスを変更された時のイベント
    public event Action<bool> ChangedBackgroundStatus = delegate { };

    //=================================================================================
    //初期化
    //=================================================================================

    //起動時に実行される
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        //オブジェクトとコンポーネントを作成し、シーン遷移では破棄されないように
        var observerObject = new GameObject("ApplicationBackgroundObserver");
        DontDestroyOnLoad(observerObject);
        Instance = observerObject.AddComponent<ApplicationBackgroundObserver>();
    }

    //=================================================================================
    //バックグラウンドにいるかのステータス切り替え
    //=================================================================================

    private void OnApplicationPause(bool pauseStatus)
    {
        ChangeBackgroundStatus(pauseStatus);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        ChangeBackgroundStatus(!hasFocus);
    }

    //アプリがバックグラウンドにいるかのステータスを変更
    private void ChangeBackgroundStatus(bool isBackground)
    {
        if (isBackground == _isBackground)
        {
            return;
        }
        _isBackground = isBackground;
        ChangedBackgroundStatus(_isBackground);
    }

}