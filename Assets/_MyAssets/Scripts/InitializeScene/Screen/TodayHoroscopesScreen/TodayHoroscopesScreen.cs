using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TodayHoroscopesScreen : BaseScreen
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

    }
    void OnClickHomeButton()
    {

    }
}
