using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DataBase;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;

public class RankingElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI subtitleText;
    [SerializeField] Button button;

    Constellation constellation;
    public void OnStart()
    {
        button.AddListener(() =>
        {
            ScreenManager.Instance.Get<RankingScreen>().Close();
            ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today, SaveDataManager.SaveData.GetCurrentCharacter());
            return UniTask.DelayFrame(0);
        });
    }

    public void Show(Fortune fortune)
    {
        constellation = CSVManager.Constellations.FirstOrDefault(constellation => constellation.id == fortune.constellation_id);
        LuckyItem luckyItem = CSVManager.LuckyItems.FirstOrDefault(luckyItem => luckyItem.id == fortune.lucky_item_id);
        LuckyColor luckyColor = CSVManager.LuckyColors.FirstOrDefault(luckyColor => luckyColor.id == fortune.lucky_color_id);
        titleText.text = $"{fortune.rank}位 {constellation.name}";
        subtitleText.text = $"ラッキーアイテム {luckyItem.name}\nラッキーカラー {luckyColor.name}";
    }
}
