using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaveDataObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPartnerProfileScreen : BaseScreen
{
    [SerializeField] Button confirmButton;
    [SerializeField] TMP_InputField nameIF;
    [SerializeField] ScrollSelector monthScrollSelector;
    [SerializeField] ScrollSelector dayScrollSelector;

    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);

        confirmButton.AddListener(async () =>
        {
            await Close();
        });

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
        Initialize.Instance.OnUpdate += OnUpdate;

    }

    public async UniTask<PartnerProfile> CreatePartner()
    {
        base.Open();
        // 一度シーンがアクティブになってから1フレーム待たないと、変更に反映されない
        await UniTask.DelayFrame(1);
        monthScrollSelector.SelectIndex(0);
        dayScrollSelector.SelectIndex(0);
        nameIF.text = "";


        await UniTask.WaitUntil(() => gameObject.activeSelf == false);

        int month = monthScrollSelector.SelectedIndex + 1;
        int day = dayScrollSelector.SelectedIndex + 1;
        string birthDay = month + "/" + day;
        // Debug.Log(birthDay);
        var birthDayDT = birthDay.ToNullableDateTime();


        return new PartnerProfile { name = nameIF.text, birthday = birthDayDT.Value };
    }

    public async UniTask<PartnerProfile> EditPartner(PartnerProfile partnerProfile)
    {
        nameIF.text = partnerProfile.name;


        base.Open();

        // 一度シーンがアクティブになってから1フレーム待たないと、変更に反映されない
        await UniTask.DelayFrame(1);
        monthScrollSelector.SelectIndex(partnerProfile.birthday.Month - 1);
        dayScrollSelector.SelectIndex(partnerProfile.birthday.Day - 1);

        await UniTask.WaitUntil(() => gameObject.activeSelf == false);

        int month = monthScrollSelector.SelectedIndex + 1;
        int day = dayScrollSelector.SelectedIndex + 1;
        string birthDay = month + "/" + day;
        // Debug.Log(birthDay);
        var birthDayDT = birthDay.ToNullableDateTime();

        return new PartnerProfile { name = nameIF.text, birthday = birthDayDT.Value };
    }

    public override UniTask Close()
    {
        return base.Close();
    }


    void OnUpdate()
    {
        int month = monthScrollSelector.SelectedIndex + 1;
        // その月の日数
        // 2004年は閏年だから
        int days = DateTime.DaysInMonth(2004, month);
        dayScrollSelector.ShowDay(days);
    }
}
