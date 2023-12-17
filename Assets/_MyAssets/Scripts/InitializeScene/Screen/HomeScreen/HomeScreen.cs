using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    [SerializeField] HomeHeader homeHeader;
    [SerializeField] HomeFooter homeFooter;
    [SerializeField] PageManager pageManager;

    [SerializeField] GameObject character;


    public override void OnStart()
    {
        base.OnStart();
        homeHeader.OnStart();
        homeFooter.OnStart();
        pageManager.OnStart();
        //character.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
        // character.SetActive(true);

    }

    public override void Close()
    {
        base.Close();
        // character.SetActive(false);
    }
}
