using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// https://docs.unity3d.com/ja/2018.4/Manual/RunningEditorCodeOnLaunch.html
/// Unityがロードされたとき、およびスクリプトが再コンパイルされたときに呼ばれる
/// </summary>
[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        // Debug.Log("Up and running");
        InputKeystore();
        FirebaseConfigManager.CreateIfNotExist();
    }



    /// <summary>
    /// Unity5 Android Build keystore 自動入力
    /// http://yasuaki-ohama.hatenablog.com/entry/2015/12/23/213956
    /// </summary>
    static void InputKeystore()
    {
        //エイリアス名
        PlayerSettings.Android.keyaliasName = "key";
        // パスワードの再設定
        PlayerSettings.Android.keystorePass = "com.ikemen";
        // パスワードの再設定
        PlayerSettings.Android.keyaliasPass = "com.ikemen";
    }
}
