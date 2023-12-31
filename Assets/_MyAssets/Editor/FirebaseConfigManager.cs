using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class FirebaseConfigManager
{
    // Androidの設定ファイル
    static readonly string ANDROID_SETTINGS = "google-services.json";
    // iOSの設定ファイル
    static readonly string IOS_SETTINGS = "GoogleService-Info.plist";
    // Firebaseの設定ファイルのパス
    static readonly string firebasePath = Application.dataPath + "/FirebaseSettingFiles/";


    public static void CreateIfNotExist()
    {
        if (!File.Exists(firebasePath + ANDROID_SETTINGS))
        {
            CreateDev();
        }
    }


    [MenuItem("MyTool/Create Firebase Settings - Prod")]
    static void CreateProd()
    {
        CreateFiles(isDev: false);
        AssetDatabase.Refresh();
    }

    [MenuItem("MyTool/Create Firebase Settings - Dev")]
    static void CreateDev()
    {
        CreateFiles(isDev: true);
        AssetDatabase.Refresh();
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
        Debug.Log("コピー開始 isDev: " + isDev);

        // google-services-desktopを更新できないため、一度消す
        try
        {
            File.Delete("Assets/StreamingAssets/google-services-desktop.json");
            Debug.Log("削除完了 google-services-desktop.json");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }


        // Firebaseの設定ファイルを上書きコピー
        try
        {
            File.Copy(firebasePath + baseAndroidSettings, firebasePath + ANDROID_SETTINGS, overwrite: true);
            Debug.Log("コピー完了 " + baseAndroidSettings);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }


        try
        {
            File.Copy(firebasePath + baseIosSettings, firebasePath + IOS_SETTINGS, overwrite: true);
            Debug.Log("コピー完了 " + baseIosSettings);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

    }
}
