using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
# if UNITY_IOS
using UnityEditor.iOS.Xcode;
# endif
using System.IO;
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Cysharp.Threading.Tasks;

public class MyBuildPostprocessor : IPreprocessBuildWithReport
{
    // 実行順
    public int callbackOrder { get { return 0; } }

    static string releaseBundleIdentifier;
    static string releaseBundleDisplayName;

    // ビルド前処理
    public async void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("OnPreprocessBuild");
        OnPreprocessBuild_Android(report);
        // 1フレーム待たないと、次のログが出ない
        await UniTask.DelayFrame(1);
        Debug.Log(EditorUserBuildSettings.development);

        FirebaseConfigManager.CreateFiles(EditorUserBuildSettings.development);
    }

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        Debug.Log("OnPostProcessBuild buildTarget : " + buildTarget);
        OnPostProcessBuild_IOS(buildTarget, path);
        OnPostProcessBuild_Android(buildTarget, path);
        // 実機に反映されるので危険
        // FirebaseConfigManager.CreateFiles(false);
    }

    public void OnPreprocessBuild_Android(BuildReport report)
    {
        //if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) return;
        Debug.Log("OnPreprocessBuild_Android");

        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            releaseBundleIdentifier = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            releaseBundleIdentifier = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
        }
        else
        {
            return;
        }

        if (releaseBundleIdentifier.Contains(".dev"))
        {
            releaseBundleIdentifier = releaseBundleIdentifier.Replace(".dev", "");
        }

        if (releaseBundleIdentifier.Contains("DefaultCompany"))
        {
            EditorUtility.DisplayDialog("MyTool", "bundle idが設定されていません", "OK");
            Debug.LogError("bundle idが設定されていません");
        }


        // ビルド時に一時的にPlayerSettingを変更し、ビルド後に戻す
        // androidでのポストプロセスが実装できなかったため、この方法ならOSに関わらず実装できる
        // TODO: ただし、ビルド中にgitの変更に出るので、間違ってコミットする可能性あり
        if (EditorUserBuildSettings.development)
        {
            releaseBundleDisplayName = PlayerSettings.productName;

            string dateName = DateTime.Today.ToString("MMdd");

            string debugBundleDisplayName = $"{dateName}_{releaseBundleDisplayName}";
            string debugBundleIdentifier = releaseBundleIdentifier + ".dev";

            PlayerSettings.productName = debugBundleDisplayName;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, debugBundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, debugBundleIdentifier);
        }
        AssetDatabase.SaveAssets();
    }

    static void OnPostProcessBuild_Android(BuildTarget buildTarget, string path)
    {
        // if (buildTarget != BuildTarget.Android) return;
        // https://qiita.com/ckazu/items/07dff39449e9f544b038
        Debug.Log("OnPostProcessBuild_Android");

        if (EditorUserBuildSettings.development)
        {
            PlayerSettings.productName = releaseBundleDisplayName;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, releaseBundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, releaseBundleIdentifier);
        }
        // PostProcessBuild後の保存が自動でされず、gitの変更にPreprocessBuildの変更が出てしまうため
        AssetDatabase.SaveAssets();
    }

    static void OnPostProcessBuild_IOS(BuildTarget buildTarget, string path)
    {
        if (buildTarget != BuildTarget.iOS) return;
#if UNITY_IOS
        string projectPath = PBXProject.GetPBXProjectPath(path);

        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        //Exception: Calling TargetGuidByName with name='Unity-iPhone' is deprecated.【解決策】
        //https://koujiro.hatenablog.com/entry/2020/03/16/050848
        string target = pbxProject.GetUnityMainTargetGuid();


        //pbxProject.AddCapability(target, PBXCapabilityType.InAppPurchase);

        // Plistの設定のための初期化
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // 輸出コンプライアンス対策
        // https://mushikago.com/i/?p=9640
        plist.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);

        // ATTのバージョン
        var buildKeyMinOS = "MinimumOSVersion";
        plist.root.SetString(buildKeyMinOS, "13.0");

        // https://nobushiueshi.com/unityios%E3%83%93%E3%83%AB%E3%83%89%E6%99%82%E3%81%ABnsphotolibraryaddusagedescription%E3%81%A8nsphotolibraryusagedescription%E3%82%92%E8%87%AA%E5%8B%95%E3%81%A7%E8%A8%AD%E5%AE%9A%E3%81%99%E3%82%8B/
        // plist.root.SetString("NSPhotoLibraryUsageDescription", "アプリに表示・使用するための写真や動画をライブラリから選択する目的でリクエストします。");
        // plist.root.SetString("NSPhotoLibraryAddUsageDescription", "アプリに表示・使用するための写真や動画を保存する目的でリクエストします。");
        // ステータスバーの文字色が背景の白に埋もれるため
        // plist.root.SetString("UIUserInterfaceStyle", "Light");

        if (EditorUserBuildSettings.development)
        {
            //日付とか
            //string dateName = DateTime.Today.ToString("MMdd");

            //アプリ名
            // plist.root.SetString("CFBundleDisplayName", $"{dateName}_{Application.productName}");

            //bundleId
            // pbxProject.SetBuildProperty(target, "PRODUCT_BUNDLE_IDENTIFIER", Application.identifier + ".dev");
        }

        // 第三引数を入れたらエラーで止まる
        // https://qiita.com/From_F/items/1994853ff90824b6bd2f
        // バックグラウンド処理の設定
        // アプリをバックグラウンドに移行しても、api通信が止まらないように
        //pbxProject.AddCapability(target, PBXCapabilityType.BackgroundModes);
        //pbxProject.AddCapability(target, PBXCapabilityType.BackgroundModes, "Background fetch");
        //pbxProject.AddCapability(target, PBXCapabilityType.BackgroundModes, "Remote notifications");

        plist.WriteToFile(plistPath);
        pbxProject.WriteToFile(projectPath);
#endif
    }
}