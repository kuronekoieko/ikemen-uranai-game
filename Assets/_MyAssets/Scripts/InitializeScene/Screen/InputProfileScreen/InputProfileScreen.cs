using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputProfileScreen : BaseScreen
{

    [SerializeField] ScrollSelector monthScrollSelector;
    [SerializeField] ScrollSelector dayScrollSelector;
    [SerializeField] Button nextButton;

    public override void OnStart()
    {
        base.OnStart();

        var months = new List<string>();
        for (int i = 1; i <= 12; i++)
        {
            months.Add(i.ToString());
        }

        var days = new List<string>();
        for (int i = 1; i <= 31; i++)
        {
            days.Add(i.ToString());
        }

        monthScrollSelector.OnStart(months, 0);
        dayScrollSelector.OnStart(days, 0);
        nextButton.onClick.AddListener(OnClickNextButton);
    }

    public override void Open()
    {
        base.Open();
    }

    void OnClickNextButton()
    {
        Close();
        ScreenManager.Instance.Get<TodayHoroscopesScreen>().Open();
    }

    public override void Close()
    {
        base.Close();
    }
}
