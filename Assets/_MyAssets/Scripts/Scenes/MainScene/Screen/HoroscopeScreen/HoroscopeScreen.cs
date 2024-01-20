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
    [SerializeField] Image characterImage;
    [SerializeField] Image constellationImage;
    Sprite defaultCharacterSprite;
    Sprite defaultConstellationSprite;


    [SerializeField] Button replayButton;
    [SerializeField] Button otherConstellationInfoButton;
    [SerializeField] Button homeButton;

    public override void OnStart()
    {
        base.OnStart();
        otherConstellationInfoButton.onClick.AddListener(OnClickOtherConstellationInfoButton);
        homeButton.onClick.AddListener(OnClickHomeButton);
        replayButton.onClick.AddListener(OnClickReplayButton);
        defaultCharacterSprite = characterImage.sprite;
        defaultConstellationSprite = constellationImage.sprite;
    }

    AudioClip audioClip;

    public async void Open(Constellation constellation, DateTime dateTime, SaveDataObjects.Character character)
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

        string fileName = AssetBundleLoader.GetAudioFileName(character, fortune);

        Debug.Log(fileName);
        audioClip = await AssetBundleLoader.LoadAssetAsync<AudioClip>(fileName);

        // TODO: 失敗したとき
        // var (fortunes, audioClip) = await UniTask.WhenAll(task1, task2);
        ScreenManager.Instance.Get<LoadingScreen>().Close();

        base.Open();

        string day = dateTime.Date == DateTime.Today ? "今日" : "明日";

        screenTitleText.text = day + "の星座占い";


        ShowConstellation(constellation);
        ShowFortune(fortune, character);
        ShowCharacter(character);

        if (audioClip != null)
        {
            AudioManager.Instance.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogError("音声がありません");
        }
    }



    async void ShowConstellation(Constellation constellation)
    {
        // iconImage.sprite=
        constellationNameText.text = "XXXX座/XXXX\n(XX/XX~XX/XX)";

        if (constellation == null) return;
        if (constellation.StartDT == null) return;
        if (constellation.EndDT == null) return;

        string name = constellation.name;
        string latin_name = constellation.latin_name;
        string start = constellation.StartDT.Value.ToString("M/d");
        string end = constellation.EndDT.Value.ToString("M/d");
        constellationNameText.text = $"{name}/{latin_name}\n({start}~{end})";

        string address = AssetBundleLoader.GetConstellationsFullAddress(constellation.id);
        Sprite sprite = await AssetBundleLoader.LoadAssetAsync<Sprite>(address);
        if (sprite != null)
        {
            constellationImage.sprite = sprite;
        }
        else
        {
            constellationImage.sprite = defaultConstellationSprite;
        }
    }

    void ShowFortune(Fortune fortune, SaveDataObjects.Character character)
    {
        fortuneRankText.text = "XX" + "位";
        luckyItemText.text = "XXXX";
        luckyColorText.text = "XXXX";
        messageText.text = "XXXX";

        if (fortune == null) return;

        fortuneRankText.text = fortune.rank + "位";

        LuckyItem luckyItem = FortuneManager.GetLuckyItem(fortune.lucky_item_id);
        if (luckyItem != null)
        {
            luckyItemText.text = luckyItem.name;
        }

        LuckyColor luckyColor = FortuneManager.GetLuckyColor(fortune.lucky_color_id);
        if (luckyColor != null)
        {
            luckyColorText.text = luckyColor.name.ToNonNull();
        }

        var message = FortuneManager.GetFortuneMessage(character, fortune.rank);
        if (string.IsNullOrEmpty(message) == false)
        {
            messageText.text = message;
        }
    }

    async void ShowCharacter(SaveDataObjects.Character character)
    {
        string address = AssetBundleLoader.GetCharacterFullAddress(character.id);
        Sprite sprite = await AssetBundleLoader.LoadAssetAsync<Sprite>(address);
        if (sprite != null)
        {
            characterImage.sprite = sprite;
        }
        else
        {
            characterImage.sprite = defaultCharacterSprite;
        }
    }


    void OnClickOtherConstellationInfoButton()
    {
        Close();
        ScreenManager.Instance.Get<RankingScreen>().Open();
    }
    void OnClickHomeButton()
    {
        Close();
        ScreenManager.Instance.Get<HomeScreen>().Open();
    }

    void OnClickReplayButton()
    {
        if (audioClip != null)
        {
            AudioManager.Instance.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogError("音声がありません");
        }
    }

    public override void Close()
    {
        base.Close();
    }
}
