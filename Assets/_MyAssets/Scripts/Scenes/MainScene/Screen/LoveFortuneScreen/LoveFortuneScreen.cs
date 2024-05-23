using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using DataBase;
using Cysharp.Threading.Tasks;
using SaveDataObjects;

public class LoveFortuneScreen : BaseScreen
{
    // ヘッダー =============================
    [SerializeField] TextMeshProUGUI screenTitleText;

    // ボディー =============================
    [SerializeField] LoveFortunePerson myLFP;
    [SerializeField] LoveFortunePerson partnerLFP;
    [SerializeField] TextMeshProUGUI compatibilityRateText;

    [SerializeField] LayoutElement compatibilityLE;
    [SerializeField] TextMeshProUGUI compatibilityTitleText;
    [SerializeField] TextMeshProUGUI compatibilityMessageText;

    [SerializeField] LayoutElement loveFortuneLE;
    [SerializeField] TextMeshProUGUI loveFortuneTitleText;
    [SerializeField] TextMeshProUGUI loveFortuneMessageText;
    [SerializeField] LoveFortuneStar loveFortuneStar;

    // フッター =============================

    [SerializeField] Button replayButton;
    [SerializeField] Button otherPartnerButton;
    [SerializeField] Button homeButton;

    AudioClip audioClip;


    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
        homeButton.AddListener(OnClickHomeButton, AudioID.BtnClick_Negative);
        replayButton.AddListener(OnClickReplayButton);
        otherPartnerButton.AddListener(OnClickOtherPartnerButton);
    }

    public void Open(PartnerProfile partnerProfile)
    {
        base.Open();


        var dateTime = DateTime.Today;
        string day = dateTime.Date == DateTime.Today ? "今日" : "明日";
        screenTitleText.text = day + "の恋愛占い";

        myLFP.Show(null, SaveDataManager.SaveData.name);

        if (partnerProfile == null)
        {
            partnerLFP.Show(null, "相手の名前");

            compatibilityRateText.text = "100" + "%";
            compatibilityMessageText.text = "相性占いの結果の文章";
            loveFortuneMessageText.text = "今日の恋愛占いの結果の文章";

            loveFortuneStar.Show(5f);
        }
        else
        {
            partnerLFP.Show(null, partnerProfile.name);

            compatibilityRateText.text = "100" + "%";
            compatibilityMessageText.text = "相性占いの結果の文章";
            loveFortuneMessageText.text = "今日の恋愛占いの結果の文章";

            loveFortuneStar.Show(5f);
        }


    }

    public override UniTask Close()
    {
        return base.Close();
    }

    async UniTask OnClickHomeButton()
    {
        await ScreenManager.Instance.Get<InputPartnerScreen>().Close();
        await Close();
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

    UniTask OnClickOtherPartnerButton()
    {
        Close();
        ScreenManager.Instance.Get<InputPartnerScreen>().Open();
        return UniTask.DelayFrame(0);
    }
}
