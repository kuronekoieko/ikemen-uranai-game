using System.Collections;
using System.Collections.Generic;
using Naninovel;
using UnityEngine;

public static class WebView
{
    public static void OpenURL(string url)
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
}
