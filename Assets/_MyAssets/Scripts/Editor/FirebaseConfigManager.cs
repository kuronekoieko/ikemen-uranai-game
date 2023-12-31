using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using UnityEditor;


public class FirebaseConfigManager
{
    // Androidの設定ファイル
    static readonly string ANDROID_SETTINGS = "google-services.json";
    // iOSの設定ファイル
    static readonly string IOS_SETTINGS = "GoogleService-Info.plist";
    // Firebaseの設定ファイルのパス
    static readonly string firebasePath = Application.dataPath + "/_MyAssets/FirebaseSettingFiles/";


    [MenuItem("MyTool/Create Firebase Settings - Prod")]
    static void CreateProd()
    {
        CreateFiles(isDev: false);
    }

    [MenuItem("MyTool/Create Firebase Settings - Dev")]
    static void CreateDev()
    {
        CreateFiles(isDev: true);
    }

    public static void CreateFiles(bool isDev)
    {
        string baseAndroidSettings;
        string baseIosSettings;

        if (isDev)
        {
            baseAndroidSettings = "google-services-dev.json";
            baseIosSettings = "GoogleService-Info-dev.plist";
        }
        else
        {
            baseAndroidSettings = "google-services-prod.json";
            baseIosSettings = "GoogleService-Info-prod.plist";
        }

        // Firebaseの設定ファイルを上書きコピー
        File.Copy(firebasePath + baseAndroidSettings, firebasePath + ANDROID_SETTINGS, overwrite: true);
        File.Copy(firebasePath + baseIosSettings, firebasePath + IOS_SETTINGS, overwrite: true);

        Debug.Log("コピー完了 isDev: " + isDev);

    }

}
