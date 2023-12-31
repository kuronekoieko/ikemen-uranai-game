using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using DataBase;

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

    public async void Open(Constellation constellation, DateTime dateTime)
    {
        if (constellation == null)
        {
            ScreenManager.Instance.Get<InputProfileScreen>().Open();
            return;
        }

        base.Open();

        screenTitleText.text = "今日の星座占い";
        // SaveDataManager.SaveData.birthDay = "01/01";
        // Debug.Log(constellation);
        string fileName = "Fortunes/" + dateTime.ToString("yyyy-MM-dd") + ".csv";
        Uri uri = await FirebaseStorageManager.Instance.GetURI(fileName);
        string csv = await FirebaseStorageManager.Instance.DownloadCsvFile(uri);
        var fortunes = CSVSerializer.Deserialize<Fortune>(csv);

        ShowConstellation(constellation);
        ShowFortune(constellation, fortunes);

        uri = await FirebaseStorageManager.Instance.GetURI("test-001.wav");
        var audioClip = await FirebaseStorageManager.Instance.DownloadAudio(uri);
        AudioManager.Instance.PlayOneShot(audioClip);
    }

    void ShowConstellation(Constellation constellation)
    {
        // iconImage.sprite=
        constellationNameText.text = "XXXX座(XX/XX~XX/XX)";

        if (constellation == null) return;
        if (constellation.StartDT == null) return;
        if (constellation.EndDT == null) return;

        string name = constellation.name;
        string start = constellation.StartDT.Value.ToString("M/d");
        string end = constellation.EndDT.Value.ToString("M/d");
        constellationNameText.text = $"{name}({start}~{end})";
    }

    void ShowFortune(Constellation constellation, Fortune[] fortunes)
    {
        fortuneRankText.text = "XX" + "位";
        luckyItemText.text = "XXXX";
        luckyColorText.text = "XXXX";
        messageText.text = "XXXX";

        if (constellation == null) return;

        var fortune = fortunes.FirstOrDefault(f => f.constellation_id == constellation.id);

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
        // Close();
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
