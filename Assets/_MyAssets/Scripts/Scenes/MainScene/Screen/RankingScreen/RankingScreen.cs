using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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


    public override UniTask Close()
    {
        base.Close();
        return UniTask.DelayFrame(0);
    }

    public override void OnStart()
    {
        base.OnStart();
        rankingElements = content.GetComponentsInChildren<RankingElement>();
        foreach (var rankingElement in rankingElements)
        {
            rankingElement.OnStart();
        }

        myFortuneButton.AddListener(async () =>
        {
            await Close();
            var constellation = SaveDataManager.SaveData.Constellation;
            ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today, SaveDataManager.SaveData.GetCurrentCharacter());
        });

        homeButton.AddListener(async () =>
        {
            await Close();
            ScreenManager.Instance.Get<HomeScreen>().Open();
        });
    }

    public override void Open()
    {
        base.Open();

        titleText.text = DateTime.Now.ToString("M/d") + DateTime.Now.ToString("ddd") + " 今日の運勢ランキング";

        var dailyFortunes = CSVManager.Fortunes
            .Where(fortune => fortune.date_time == DateTime.Today.ToDateKey())
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
