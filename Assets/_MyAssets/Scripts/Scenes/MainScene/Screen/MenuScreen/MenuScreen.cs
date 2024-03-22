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

    public override UniTask Close()
    {
        base.Close();
        return UniTask.DelayFrame(0);
    }

    UniTask OnClick_HowToPlay(string title)
    {
        ScreenManager.Instance.Get<WebViewScreen>().Open(title, URLs.HowToPlay);
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_QA(string title)
    {
        ScreenManager.Instance.Get<WebViewScreen>().Open(title, URLs.QA);
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_X(string title)
    {
        Application.OpenURL(URLs.X);
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_AppReview(string title)
    {
        InAppReviewManager.RequestReview();
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_ContactUs(string title)
    {
        ScreenManager.Instance.Get<WebViewScreen>().Open(title, URLs.ContactUs);
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_Notification(string title)
    {
        ScreenManager.Instance.Get<NotificationSettingScreen>().Open(title);
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_Location(string title)
    {
        LocationService.Start();
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_Account(string title)
    {
        ScreenManager.Instance.Get<WebViewScreen>().Open(title, URLs.Account);
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_TermsOfUse(string title)
    {
        ScreenManager.Instance.Get<WebViewScreen>().Open(title, URLs.TermsOfUse);
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_PrivacyPolicy(string title)
    {
        ScreenManager.Instance.Get<WebViewScreen>().Open(title, URLs.PrivacyPolicy);
        return UniTask.DelayFrame(0);
    }
    UniTask OnClick_Disclaimer(string title)
    {
        // 特商・資金決済法の表記
        ScreenManager.Instance.Get<WebViewScreen>().Open(title, URLs.Disclaimer);
        return UniTask.DelayFrame(0);
    }
}
