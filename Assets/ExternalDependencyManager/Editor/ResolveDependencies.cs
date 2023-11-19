using Google.JarResolver;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ResolveDependencies
{
    static ResolveDependencies()
    {
        var support = PlayServicesSupport.CreateInstance
        (
            clientName: "GooglePlayGames",
            sdkPath: EditorPrefs.GetString("AndroidSdkRoot"),
            settingsDirectory: "ProjectSettings"
        );

        support.DependOn
        (
            group: "com.android.support",
            artifact: "customtabs",
            version: "23.0.0"
        );
    }
}