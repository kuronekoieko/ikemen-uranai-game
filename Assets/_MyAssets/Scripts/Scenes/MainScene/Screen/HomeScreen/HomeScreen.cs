using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class HomeScreen : BaseScreen
{
    [SerializeField] HomeHeader homeHeader;
    [SerializeField] HomeFooter homeFooter;
    [SerializeField] PageManager pageManager;

    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
        // フッターの初期化でページ遷移したいので、pageManagerが先
        pageManager.OnStart();
        homeHeader.OnStart();
        homeFooter.OnStart();
    }

    public override void Open()
    {
        AudioManager.Instance.Play(AudioID.Home);
        base.Open();
    }

    public override UniTask Close()
    {
        return base.Close();
    }
}
