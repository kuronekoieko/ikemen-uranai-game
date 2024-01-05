using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DataBase;
using System.Linq;

public class RankingElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI subtitleText;


    public void OnStart()
    {

    }

    public void OnOpen()
    {

    }

    public void Show(Fortune fortune)
    {
        Constellation constellation = CSVManager.Instance.Constellations.FirstOrDefault(constellation => constellation.id == fortune.constellation_id);
        LuckyItem luckyItem = CSVManager.Instance.LuckyItems.FirstOrDefault(luckyItem => luckyItem.id == fortune.lucky_item_id);
        LuckyColor luckyColor = CSVManager.Instance.LuckyColors.FirstOrDefault(luckyColor => luckyColor.id == fortune.lucky_color_id);
        titleText.text = $"{fortune.rank}位 {constellation.name}";
        subtitleText.text = $"ラッキーアイテム {luckyItem.name}\nラッキーカラー {luckyColor.name}";
    }
}
