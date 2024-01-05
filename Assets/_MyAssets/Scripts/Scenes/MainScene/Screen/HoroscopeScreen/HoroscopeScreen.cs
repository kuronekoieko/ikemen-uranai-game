using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using DataBase;
using Cysharp.Threading.Tasks;

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

    public async void Open(Constellation constellation, DateTime dateTime, string characterId)
    {
        if (constellation == null)
        {
            ScreenManager.Instance.Get<InputProfileScreen>().Open();
            return;
        }
        ScreenManager.Instance.Get<LoadingScreen>().Open();

        Fortune fortune = FortuneManager.GetFortune(dateTime, constellation.id);
        // TODO: 失敗したとき
        if (fortune == null) return;


        // var task1 = FileDownloader.GetFortunes(dateTime);
        string fileName = FileDownloader.GetAudioFileName(characterId, fortune);

        Debug.Log(fileName);
        var audioClip = await FileDownloader.GetAudioClip(fileName);

        // TODO: 失敗したとき
        // var (fortunes, audioClip) = await UniTask.WhenAll(task1, task2);
        ScreenManager.Instance.Get<LoadingScreen>().Close();

        base.Open();

        string day = dateTime.Date == DateTime.Today ? "今日" : "明日";

        screenTitleText.text = day + "の星座占い";


        ShowConstellation(constellation);
        ShowFortune(constellation, fortune, characterId);


        if (audioClip != null)
        {
            AudioManager.Instance.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogError("音声がありません");
        }
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

    void ShowFortune(Constellation constellation, Fortune fortune, string characterId)
    {
        fortuneRankText.text = "XX" + "位";
        luckyItemText.text = "XXXX";
        luckyColorText.text = "XXXX";
        messageText.text = "XXXX";

        if (constellation == null) return;

        fortuneRankText.text = fortune.rank + "位";

        LuckyItem luckyItem = CSVManager.Instance.LuckyItems.FirstOrDefault(luckyItem => luckyItem.id == fortune.lucky_item_id);
        if (luckyItem != null && !string.IsNullOrEmpty(luckyItem.name))
        {
            luckyItemText.text = luckyItem.name.ToNonNull();
        }
        else
        {
            luckyItemText.text = "ラッキーアイテム";
        }

        LuckyColor luckyColor = CSVManager.Instance.LuckyColors.FirstOrDefault(luckyColor => luckyColor.id == fortune.lucky_color_id);
        if (luckyColor != null && !string.IsNullOrEmpty(luckyColor.name))
        {
            luckyColorText.text = luckyColor.name.ToNonNull();
        }
        else
        {
            luckyColorText.text = "ラッキーカラー";
        }

        var dataBaseCharacter = CSVManager.Instance.Characters.FirstOrDefault(c => c.id == SaveDataManager.SaveData.currentCharacterId);
        if (dataBaseCharacter == null) return;

        var fortuneMessage = CSVManager.Instance.FortuneMessages
            .Where(f => f.character_id == characterId)
            .FirstOrDefault(f => f.rank == fortune.rank);

        if (fortuneMessage == null) return;
        // TODO: ロジック実装
        messageText.text = fortuneMessage.messages.GetRandom();
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
