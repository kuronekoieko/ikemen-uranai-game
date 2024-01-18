using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mopsicus.Plugins;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class MenuScreen : BaseScreen
{
    [SerializeField] MenuElementPool menuElementPool;



    public override void OnStart()
    {
        base.OnStart();
        menuElementPool.OnStart();

        var menuElementObjs = new List<MenuElementObj>();
        menuElementObjs.Add(new() { title = "遊び方", onClick = OnClick_HowToPlay });
        menuElementObjs.Add(new() { title = "よくある質問", onClick = OnClick_QA });
        menuElementObjs.Add(new() { title = "公式Twitterをフォローする", onClick = OnClick_X });
        menuElementObjs.Add(new() { title = "アプリをレビューする", onClick = OnClick_AppReview });
        menuElementObjs.Add(new() { title = "サポートへ問い合わせ", onClick = OnClick_ContactUs });
        menuElementObjs.Add(new() { title = "通知の設定", onClick = OnClick_Notification });
        menuElementObjs.Add(new() { title = "位置情報の設定", onClick = OnClick_Location });
        menuElementObjs.Add(new() { title = "機種変更", onClick = OnClick_Account });
        menuElementObjs.Add(new() { title = "利用規約", onClick = OnClick_TermsOfUse });
        menuElementObjs.Add(new() { title = "プライバシーポリシー", onClick = OnClick_PrivacyPolicy });
        menuElementObjs.Add(new() { title = "特商・資金決済法の表記", onClick = OnClick_Disclaimer });

        menuElementPool.Show(menuElementObjs.ToArray());
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    void OnClick_HowToPlay()
    {
        // TODO: webview
        Application.OpenURL("https://ikebo.jp/howto");
    }

    void OnClick_QA()
    {
        // TODO: webview
        Application.OpenURL("https://ikebo.jp/faq");
    }
    void OnClick_X()
    {
        Application.OpenURL("https://twitter.com/ikebo");

    }
    void OnClick_AppReview()
    {
        InAppReviewManager.RequestReview();
    }
    void OnClick_ContactUs()
    {
        // TODO: webview
        Application.OpenURL("https://ikebo.jp/support");
    }
    void OnClick_Notification()
    {
        ScreenManager.Instance.Get<NotificationSettingScreen>().Open();
    }
    async void OnClick_Location()
    {
        await LocationService.Start();
    }
    void OnClick_Account()
    {
        // TODO: webview
        Application.OpenURL("https://ikebo.jp/transfer");

    }
    void OnClick_TermsOfUse()
    {
        // TODO: webview
        Application.OpenURL("https://ikebo.jp/kiyaku");

    }
    void OnClick_PrivacyPolicy()
    {
        // TODO: webview
        Application.OpenURL("https://ikebo.jp/privacypolicy");

    }
    void OnClick_Disclaimer()
    {
        // TODO: webview
        // 特商・資金決済法の表記
        Application.OpenURL("https://ikebo.jp/disclaimer");
    }
}
