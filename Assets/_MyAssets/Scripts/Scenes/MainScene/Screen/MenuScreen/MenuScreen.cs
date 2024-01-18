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
        menuElementObjs.Add(new() { title = "特商・資金決済法の表記", onClick = OnClick_CFLR });

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

    }

    void OnClick_QA()
    {

    }
    void OnClick_X()
    {

    }
    void OnClick_AppReview()
    {

    }
    void OnClick_ContactUs()
    {

    }
    void OnClick_Notification()
    {

    }
    void OnClick_Location()
    {

    }
    void OnClick_Account()
    {

    }
    void OnClick_TermsOfUse()
    {

    }
    void OnClick_PrivacyPolicy()
    {

    }
    void OnClick_CFLR()
    {
        // 特商・資金決済法の表記

    }
}
