using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://baba-s.hatenablog.com/entry/2020/11/16/080000_8
/// </summary>
public static class ChromeCustomTabs
{
    public static void OpenURL(string url)
    {
#if UNITY_EDITOR
        // Application.OpenURL(url);
        Debug.Log("open webview : " + url);
#elif UNITY_ANDROID
        using ( var unityPlayer = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ) )
        using ( var activity = unityPlayer.GetStatic<AndroidJavaObject>( "currentActivity" ) )
        using ( var intentBuilder = new AndroidJavaObject( "android.support.customtabs.CustomTabsIntent$Builder" ) )
        using ( var intent = intentBuilder.Call<AndroidJavaObject>( "build" ) )
        using ( var uriClass = new AndroidJavaClass( "android.net.Uri" ) )
        using ( var uri = uriClass.CallStatic<AndroidJavaObject>( "parse", url ) )
        {
            intent.Call( "launchUrl", activity, uri );
        }
#endif
    }
}