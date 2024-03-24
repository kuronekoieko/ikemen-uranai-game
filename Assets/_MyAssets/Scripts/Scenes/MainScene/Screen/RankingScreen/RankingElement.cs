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
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] Image constellationImage;
    [SerializeField] TextMeshProUGUI luckyColorText;
    [SerializeField] Button button;
    [SerializeField] Sprite[] constellationSprites;

    Sprite defaultConstellationSprite;
    Constellation constellation;

    public void OnStart()
    {
        button.AddListener(OnClick);
        defaultConstellationSprite = constellationImage.sprite;

    }

    public void Show(Fortune fortune)
    {
        constellation = CSVManager.Constellations.FirstOrDefault(constellation => constellation.id == fortune.constellation_id);
        // LuckyItem luckyItem = CSVManager.LuckyItems.FirstOrDefault(luckyItem => luckyItem.id == fortune.lucky_item_id);
        LuckyColor luckyColor = CSVManager.LuckyColors.FirstOrDefault(luckyColor => luckyColor.id == fortune.lucky_color_id);
        rankText.text = $"{fortune.rank}位";
        luckyColorText.text = $"{luckyColor.name}";

        bool success = int.TryParse(constellation.id, out int idInt);
        if (success == false)
        {
            constellationImage.sprite = defaultConstellationSprite;
            return;
        }
        constellationSprites.TryGetValue(idInt - 1, out Sprite sprite);
        if (sprite == null)
        {
            constellationImage.sprite = defaultConstellationSprite;
            return;
        }

        constellationImage.sprite = sprite;
    }

    async UniTask OnClick()
    {
        await ScreenManager.Instance.Get<HoroscopeScreen>().Open(constellation, DateTime.Today, SaveDataManager.SaveData.GetCurrentCharacter());
    }
}
