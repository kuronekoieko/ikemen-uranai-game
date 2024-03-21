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

    [TextArea]
    [SerializeField] string termsOfUseStr;

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
        WebView.OpenURL(URLs.HowToPlay);
    }

    void OnClick_QA()
    {
        WebView.OpenURL(URLs.QA);
    }
    void OnClick_X()
    {
        Application.OpenURL(URLs.X);
    }
    void OnClick_AppReview()
    {
        InAppReviewManager.RequestReview();
    }
    void OnClick_ContactUs()
    {
        WebView.OpenURL(URLs.ContactUs);
    }
    void OnClick_Notification()
    {
        ScreenManager.Instance.Get<NotificationSettingScreen>().Open();
    }
    void OnClick_Location()
    {
        LocationService.Start();
    }
    void OnClick_Account()
    {
        WebView.OpenURL(URLs.Account);
    }
    void OnClick_TermsOfUse()
    {
        // WebView.OpenURL(URLs.TermsOfUse);
        ScreenManager.Instance.Get<DescriptionScreen>().Open("利用規約", termsOfUseStr);
    }
    void OnClick_PrivacyPolicy()
    {
        WebView.OpenURL(URLs.PrivacyPolicy);

    }
    void OnClick_Disclaimer()
    {
        // 特商・資金決済法の表記
        WebView.OpenURL(URLs.Disclaimer);
    }
}
