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
    [SerializeField] Image constellationImage;

    // ボディー =============================
    [SerializeField] TextMeshProUGUI constellationNameText;
    [SerializeField] TextMeshProUGUI fortuneRankText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI luckyItemText;
    [SerializeField] TextMeshProUGUI luckyColorText;
    [SerializeField] Image characterImage;
    Sprite defaultCharacterSprite;
    Sprite defaultConstellationSprite;


    [SerializeField] Button replayButton;
    [SerializeField] Button otherConstellationInfoButton;
    [SerializeField] Button homeButton;
    [SerializeField] Sprite[] constellationSprites;


    public override void OnStart()
    {
        base.OnStart();
        otherConstellationInfoButton.AddListener(OnClickOtherConstellationInfoButton);
        homeButton.AddListener(OnClickHomeButton, AudioID.BtnClick_Negative);
        replayButton.AddListener(OnClickReplayButton);
        defaultCharacterSprite = characterImage.sprite;
        defaultConstellationSprite = constellationImage.sprite;
    }

    AudioClip audioClip;

    public async UniTask Open(Constellation constellation, DateTime dateTime, SaveDataObjects.Character character)
    {
        if (constellation == null)
        {
            ScreenManager.Instance.Get<InputProfileScreen>().Open();
            return;
        }
        //ScreenManager.Instance.Get<LoadingScreen>().Open();

        Fortune fortune = FortuneManager.GetFortune(dateTime, constellation.id);
        // TODO: 失敗したとき
        if (fortune == null) return;

        string fileName = AssetBundleLoader.GetAudioFileName(character, fortune);

        Debug.Log(fileName);
        audioClip = await AssetBundleLoader.LoadAssetAsync<AudioClip>(fileName);

        // TODO: 失敗したとき
        // var (fortunes, audioClip) = await UniTask.WhenAll(task1, task2);
        //await ScreenManager.Instance.Get<LoadingScreen>().Close();

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
        
        AudioManager.Instance.Play(AudioID.HoroscopeScreen);
    }



    void ShowConstellation(Constellation constellation)
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


    UniTask OnClickOtherConstellationInfoButton()
    {
        Close();
        ScreenManager.Instance.Get<RankingScreen>().Open();
        return UniTask.DelayFrame(0);
    }
    async UniTask OnClickHomeButton()
    {
        await Close();
        await ScreenManager.Instance.Get<RankingScreen>().Close();
    }

    UniTask OnClickReplayButton()
    {
        if (audioClip != null)
        {
            AudioManager.Instance.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogError("音声がありません");
        }
        return UniTask.DelayFrame(0);
    }

    public override UniTask Close()
    {
        base.Close();
        return UniTask.DelayFrame(0);
    }
}

