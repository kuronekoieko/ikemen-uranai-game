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
        var constellation = CSVManager.Instance.Constellations.FirstOrDefault(c => c.id == "01");
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
