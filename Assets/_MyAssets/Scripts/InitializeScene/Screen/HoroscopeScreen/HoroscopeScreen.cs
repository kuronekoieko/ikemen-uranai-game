using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class HoroscopeScreen : BaseScreen
{

    // ヘッダー =============================
    [SerializeField] TextMeshProUGUI screenTitleText;

    // ボディー =============================
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI constellationNameText;
    [SerializeField] TextMeshProUGUI fortuneRankText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI luckyItemText;
    [SerializeField] TextMeshProUGUI luckyColorText;

    // フッター =============================
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

        screenTitleText.text = "今日の星座占い";

        var constellation = SaveDataManager.SaveData.Constellation;

        ShowConstellation(constellation);
        ShowFortune(constellation);
    }

    void ShowConstellation(DataBase.Constellation constellation)
    {
        // iconImage.sprite=
        constellationNameText.text = "XXXX座(XX/XX~XX/XX)";

        if (constellation == null) return;

        string name = constellation.name;
        string start = constellation.StartDT.ToString("M/d");
        string end = constellation.EndDT.ToString("M/d");
        constellationNameText.text = $"{name}({start}~{end})";
    }

    void ShowFortune(DataBase.Constellation constellation)
    {
        fortuneRankText.text = "XX" + "位";
        luckyItemText.text = "XXXX";
        luckyColorText.text = "XXXX";
        messageText.text = "XXXX";

        if (constellation == null) return;

        var fortune = CSVManager.Instance.Fortunes.FirstOrDefault(f => f.constellation_id == constellation.id);

        if (fortune == null) return;

        fortuneRankText.text = fortune.rank + "位";
        luckyItemText.text = fortune.item;
        luckyColorText.text = fortune.color;

        var dataBaseCharacter = CSVManager.Instance.Characters.FirstOrDefault(c => c.id == SaveDataManager.SaveData.currentCharacterId);
        if (dataBaseCharacter == null) return;

        if (fortune.rank <= 4)
        {
            messageText.text = dataBaseCharacter.rank_message_low;
            return;
        }
        if (fortune.rank <= 8)
        {
            messageText.text = dataBaseCharacter.rank_message_mid;
            return;
        }
        if (fortune.rank <= 12)
        {
            messageText.text = dataBaseCharacter.rank_message_high;
            return;
        }
    }


    void OnClickOtherConstellationInfoButton()
    {
        Close();
    }
    void OnClickHomeButton()
    {
        Close();
        ScreenManager.Instance.Get<HomeScreen>().Open();
    }

    public override void Close()
    {
        base.Close();
    }
}
