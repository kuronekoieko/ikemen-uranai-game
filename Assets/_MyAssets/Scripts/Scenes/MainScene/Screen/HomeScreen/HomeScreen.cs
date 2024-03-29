using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class HomeScreen : SingletonMonoBehaviour<HomeScreen>
{
    [SerializeField] HomeHeader homeHeader;
    [SerializeField] HomeFooter homeFooter;
    [SerializeField] PageManager pageManager;

    public void OnStart()
    {
        // フッターの初期化でページ遷移したいので、pageManagerが先
        pageManager.OnStart();
        homeHeader.OnStart();
        homeFooter.OnStart();
    }
}
