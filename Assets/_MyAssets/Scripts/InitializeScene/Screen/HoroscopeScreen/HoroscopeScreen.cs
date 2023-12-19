using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
        // iconImage.sprite=
        string constellationId = "01";
        var constellation = CSVManager.Instance.Constellations.FirstOrDefault(c => c.id == constellationId);
        if (constellation == null)
        {
            constellationNameText.text = "XXXX座(XX/XX~XX/XX)";
        }
        else
        {
            string name = constellation.name;
            string start = constellation.StartDT.ToString("M/d");
            string end = constellation.EndDT.ToString("M/d");
            constellationNameText.text = $"{name}({start}~{end})";
        }

        var fortune = CSVManager.Instance.Fortunes.FirstOrDefault(f => f.constellation_id == constellationId);
        if (fortune == null)
        {
            fortuneRankText.text = "XX" + "位";
            luckyItemText.text = "XXXX";
            luckyColorText.text = "XXXX";
        }
        else
        {
            fortuneRankText.text = fortune.rank + "位";
            luckyItemText.text = fortune.item;
            luckyColorText.text = fortune.color;
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
