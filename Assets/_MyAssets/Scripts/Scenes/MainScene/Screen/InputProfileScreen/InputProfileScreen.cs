using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MainScene;
using Cysharp.Threading.Tasks;

public class InputProfileScreen : BaseScreen
{

    [SerializeField] ScrollSelector monthScrollSelector;
    [SerializeField] ScrollSelector dayScrollSelector;
    [SerializeField] Button nextButton;
    [SerializeField] TMP_InputField inputField;

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

        monthScrollSelector.OnStart(months);
        dayScrollSelector.OnStart(days);


        nextButton.onClick.AddListener(OnClickNextButton);
        Initialize.Instance.OnUpdate += OnUpdate;
    }

    void OnUpdate()
    {
        int month = monthScrollSelector.SelectedIndex + 1;
        // その月の日数
        // 2004年は閏年だから
        int days = DateTime.DaysInMonth(2004, month);
        dayScrollSelector.ShowDay(days);
    }

    public async override void Open()
    {
        base.Open();

        // 一度シーンがアクティブになってから1フレーム待たないと、変更に反映されない
        await UniTask.DelayFrame(1);
        monthScrollSelector.SelectIndex(0);
        dayScrollSelector.SelectIndex(0);
    }

    void OnClickNextButton()
    {
        int month = monthScrollSelector.SelectedIndex + 1;
        int day = dayScrollSelector.SelectedIndex + 1;
        string birthDay = month + "/" + day;
        // Debug.Log(birthDay);
        var birthDayDT = birthDay.ToNullableDateTime();
        if (birthDayDT == null)
        {
            Debug.Log("誕生日が入力されていない");
        }
        else
        {
            SaveDataManager.SaveData.name = inputField.text;
            SaveDataManager.SaveData.birthDay = birthDay;
            SaveDataManager.Save();
            var constellation = SaveDataManager.SaveData.Constellation;
            ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today);
            Close();
        }
    }

    public override void Close()
    {
        base.Close();
    }
}
