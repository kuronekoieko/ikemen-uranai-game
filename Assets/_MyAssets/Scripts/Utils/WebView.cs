using System.Collections;
using System.Collections.Generic;
using Gpm.WebView;
using Naninovel;
using UnityEngine;

public static class WebView
{
    public static void OpenURL_(string url)
    {
        url = url.TrimStart().TrimEnd();
        if (Application.isEditor)
        {
            Application.OpenURL(url);
            return;
        }
        var webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();

        // 初期化
        webViewObject.Init(
            // NOTE: iOSでUIWebViewではなくWKWebViewを利用する(現在はほぼ必須な設定項目だと思ってもらえれば)
            enableWKWebView: true
        );

        // URLを読み込みWebViewを表示する
        webViewObject.SetVisibility(true);
        webViewObject.LoadURL(url);

    }


    public static void OpenURL(string url)
    {
        List<string> schemeList = new()
        {
            "scheme1",
            "scheme2"
        };

        GpmWebView.ShowUrl(
         url,
         new GpmWebViewRequest.Configuration()
         {
             title = "docs|GPMWebView",
             orientation = (int)ScreenOrientation.AutoRotation,
             contentMode = GpmWebViewContentMode.RECOMMENDED
         },
         (type, data, error) =>
         {
             // close callback
             if (error == null)
             {
                 Debug.Log("close succeeded.");
             }
             else
             {
                 Debug.Log(string.Format("Close failed. code:{0}, message:{1}", error.code, error.message));
             }
         },
         schemeList
     );

    }
}
