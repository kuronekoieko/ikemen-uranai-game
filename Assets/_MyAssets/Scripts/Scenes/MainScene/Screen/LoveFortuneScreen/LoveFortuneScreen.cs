using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using DataBase;
using Cysharp.Threading.Tasks;

public class LoveFortuneScreen : BaseScreen
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


    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
    }

    public override void Open()
    {
        base.Open();
    }

    public override UniTask Close()
    {
        return base.Close();
    }
}
