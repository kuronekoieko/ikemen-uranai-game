using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataBase;
using UnityEngine;

public class RankingScreen : BaseScreen
{
    [SerializeField] Transform content;
    RankingElement[] rankingElements;
    public override void Close()
    {
        base.Close();
    }

    public override void OnStart()
    {
        base.OnStart();
        rankingElements = content.GetComponentsInChildren<RankingElement>();
    }

    public override void Open()
    {
        base.Open();

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
