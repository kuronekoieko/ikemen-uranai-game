using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]//エディター起動時に初期化されるように
public class UnitypackageEvents
{
    //コンストラクタ(InitializeOnLoad属性によりエディター起動時に呼び出される)
    static UnitypackageEvents()
    {
        AssetDatabase.importPackageCompleted += ImportCompleted;
        AssetDatabase.importPackageCancelled += ImportCancelled;
        AssetDatabase.importPackageFailed += ImportCallBackFailed;
        AssetDatabase.importPackageStarted += ImportStarted;
    }

    private static void ImportStarted(string packageName)
    {
        // Debug.Log(packageName + "のインポート開始");
    }

    private static void ImportCancelled(string packageName)
    {
        // Debug.Log(packageName + "のインポートキャンセル");
    }

    private static void ImportCallBackFailed(string packageName, string _error)
    {
        // Debug.Log(packageName + "のインポート失敗 : " + _error);
    }

    private static void ImportCompleted(string packageName)
    {
        // Debug.Log(packageName + "のインポート完了 " + MyToolPropertySO.Instance.IsCompleteInitialize);
        // if (!packageName.Contains("MyTool")) return;
        // if (MyToolPropertySO.Instance.IsCompleteInitialize) return;
        // MyToolPropertySO.Instance.IsCompleteInitialize = true;
        // SetPlayerSettings();
    }




}
