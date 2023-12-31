using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class PlayerSettingsInitializer
{
    [MenuItem("MyTool/PlayerSettings Initializer")]
    static void OpenPlayerSettingsSetterDialog()
    {
        string message = "PlayerSettingsを一括設定します。";
        if (EditorUtility.DisplayDialog("注意", message, "OK", "キャンセル"))
        {
            SetPlayerSettings();
        }
    }

    static void SetPlayerSettings()
    {
        // 共通 -----------------------------------------------
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.muteOtherAudioSources = false;

        // iOS ------------------------------------------------        
        PlayerSettings.iOS.buildNumber = "0.1";
        PlayerSettings.iOS.appleDeveloperTeamID = "M6UDRCH87S";
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneOnly;
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1);// ARM64

        // Android --------------------------------------------
        // 64bit対応
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        // AndroidSdkVersionsに31が無いため(2022/05/30)
        // PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)Enum.ToObject(typeof(AndroidSdkVersions), 31);
        // https://developer.android.com/google/play/requirements/target-sdk?hl=ja
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;
        PlayerSettings.Android.bundleVersionCode = 1;

        // 変更の自動保存
        AssetDatabase.SaveAssets();
        Debug.Log("Changed PlayerSettings");
    }
}
