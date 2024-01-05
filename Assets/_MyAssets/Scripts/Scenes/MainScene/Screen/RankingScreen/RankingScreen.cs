using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingScreen : BaseScreen
{
    [SerializeField] Transform content;
    RankingElement[] rankingElements;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Button myFortuneButton;
    [SerializeField] Button homeButton;


    public override void Close()
    {
        base.Close();
    }

    public override void OnStart()
    {
        base.OnStart();
        rankingElements = content.GetComponentsInChildren<RankingElement>();
        foreach (var rankingElement in rankingElements)
        {
            rankingElement.OnStart();
        }

        myFortuneButton.onClick.AddListener(() =>
        {
            var constellation = SaveDataManager.SaveData.Constellation;
            ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today, SaveDataManager.SaveData.currentCharacterId);
        });

        homeButton.onClick.AddListener(() =>
        {
            Close();
            ScreenManager.Instance.Get<HomeScreen>().Open();
        });
    }

    public override void Open()
    {
        base.Open();

        titleText.text = DateTime.Now.ToString("M/d") + DateTime.Now.ToString("ddd") + " 今日の運勢ランキング";

        var dailyFortunes = CSVManager.Instance.Fortunes
            .Where(fortune => fortune.date_time == DateTime.Today.ToStringDate())
            .OrderBy(fortune => fortune.rank)
            .ToArray();

        for (int i = 0; i < dailyFortunes.Length; i++)
        {
            Fortune fortune = dailyFortunes[i];
            RankingElement rankingElement = rankingElements[i];

            rankingElement.Show(fortune);
        }
    }
}
