using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoroscopeScreen : BaseScreen
{
    [SerializeField] Button otherConstellationInfoButton;
    [SerializeField] Button homeButton;


    public override void OnStart()
    {
        base.OnStart();
        otherConstellationInfoButton.onClick.AddListener(OnClickOtherConstellationInfoButton);
        homeButton.onClick.AddListener(OnClickHomeButton);
    }

    public override void Open()
    {
        base.Open();
    }

    void OnClickOtherConstellationInfoButton()
    {
        Close();
    }
    void OnClickHomeButton()
    {
        Close();
        ScreenManager.Instance.Get<HomeScreen>().Open();
    }

    public override void Close()
    {
        base.Close();
    }
}
