using System.Collections;
using System.Collections.Generic;
using Gpm.WebView;
using Naninovel;
using UnityEngine;

public static class WebView
{
    public static void OpenURL(string url)
    {
        int height = (int)CanvasManager.Instance.CanvasScaler.referenceResolution.y;
        int width = (int)CanvasManager.Instance.CanvasScaler.referenceResolution.x;
        // OpenURL(url, width, height);
    }

    public static void Close()
    {
        if (Application.isEditor)
        {
            //Application.OpenURL(url);
            DummyWebview.Instance.Close();
            return;
        }
        GpmWebView.Close();

    }

    public static void OpenURL(string url, RectInt rect)
    {
        if (Application.isEditor)
        {
            //Application.OpenURL(url);
            DummyWebview.Instance.Show(rect);
            return;
        }

        // https://github.com/nhn/gpm.unity/blob/main/docs/WebView/README.en.md
        GpmWebView.ShowUrl(
            url,
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.PORTRAIT,
                isClearCookie = true,
                isClearCache = true,
                backgroundColor = "#FFFFFF",
                isNavigationBarVisible = false,
                navigationBarColor = "#4B96E6",
                title = "",
                isBackButtonVisible = true,
                isForwardButtonVisible = true,
                isCloseButtonVisible = true,
                supportMultipleWindows = true,
#if  UNITY_IOS
                contentMode = GpmWebViewContentMode.MOBILE
#endif
            },
            OnCallback,
            new List<string>()
            {
                "USER_ CUSTOM_SCHEME"
            });

        GpmWebView.SetMargins(0, 0, 0, 0);
        // 左上原点
        GpmWebView.SetPosition(rect.x, rect.y);
        GpmWebView.SetSize(rect.width, rect.height);


        Debug.Log("GpmWebView: " + GpmWebView.GetX());
        Debug.Log("GpmWebView: " + GpmWebView.GetY());
        Debug.Log("GpmWebView: " + GpmWebView.GetWidth());
        Debug.Log("GpmWebView: " + GpmWebView.GetHeight());

    }

    static void OnCallback(
        GpmWebViewCallback.CallbackType callbackType,
        string data,
        GpmWebViewError error)
    {
        Debug.Log("OnCallback: " + callbackType);
        switch (callbackType)
        {
            case GpmWebViewCallback.CallbackType.Open:
                if (error != null)
                {
                    Debug.LogFormat("Fail to open WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.Close:
                if (error != null)
                {
                    Debug.LogFormat("Fail to close WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageStarted:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("PageStarted Url : {0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageLoad:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("Loaded Page:{0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowOpen:
                Debug.Log("MultiWindowOpen");
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowClose:
                Debug.Log("MultiWindowClose");
                break;
            case GpmWebViewCallback.CallbackType.Scheme:
                if (error == null)
                {
                    if (data.Equals("USER_ CUSTOM_SCHEME") == true || data.Contains("CUSTOM_SCHEME") == true)
                    {
                        Debug.Log(string.Format("scheme:{0}", data));
                    }
                }
                else
                {
                    Debug.Log(string.Format("Fail to custom scheme. Error:{0}", error));
                }
                break;
            case GpmWebViewCallback.CallbackType.GoBack:
                Debug.Log("GoBack");
                break;
            case GpmWebViewCallback.CallbackType.GoForward:
                Debug.Log("GoForward");
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                Debug.LogFormat("ExecuteJavascript data : {0}, error : {1}", data, error);
                break;
#if UNITY_ANDROID
        case GpmWebViewCallback.CallbackType.BackButtonClose:
            Debug.Log("BackButtonClose");
            break;
#endif
        }
    }
}
