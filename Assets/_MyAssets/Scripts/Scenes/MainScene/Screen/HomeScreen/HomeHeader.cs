using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Globalization;
using MainScene;
using System.Linq;

public class HomeHeader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerLevelText;
    [SerializeField] Image playerExpBarImage;
    [SerializeField] TextMeshProUGUI playerExpPerText;

    [SerializeField] TextMeshProUGUI jemFreeText;
    [SerializeField] TextMeshProUGUI jemChargingText;

    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] TextMeshProUGUI characterLevelText;
    [SerializeField] TextMeshProUGUI characterExpPerText;
    [SerializeField] Image characterExpBarImage;

    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Button chargingScreenButton;
    [SerializeField] Button openMenuScreenButton;


    SaveData SaveData => SaveDataManager.SaveData;


    public void OnStart()
    {
        chargingScreenButton.onClick.AddListener(OnClickChargingScreenButton);
        openMenuScreenButton.onClick.AddListener(OnClickOpenMenuScreenButton);
        Initialize.Instance.OnUpdate += OnUpdate;
    }

    private void OnUpdate()
    {
        // セーブデータが別のインスタンスをつくってしまうから？
        // セーブデータがロードされないため
        if (SaveData == null) return;

        playerLevelText.text = "Lv." + SaveData.level.ToString();
        var levelData = GetCurrentLevelData_Player(SaveData.level);
        playerExpBarImage.fillAmount = (float)SaveData.exp / (float)levelData.exp;
        playerExpPerText.text = Mathf.FloorToInt(playerExpBarImage.fillAmount * 100) + "%";

        jemFreeText.text = SaveData.jemFree.ToString();
        jemChargingText.text = SaveData.jemCharging.ToString();
        dayText.text = DateTime.Now.ToString("MM d ddd", CultureInfo.CreateSpecificCulture("en-US"));
        timeText.text = DateTime.Now.ToString("HH:mm");

        ShowExp();

        var characterData = GetCharacterData(SaveData.currentCharacterId);
        if (characterData == null)
        {
            characterNameText.text = "キャラクター";
        }
        else
        {
            characterNameText.text = characterData.name_jp;
        }

    }

    void ShowExp()
    {
        var currentCharacter = SaveData.GetCurrentCharacter();
        if (currentCharacter == null)
        {
            characterLevelText.text = "Lv." + "999";
            characterExpPerText.text = "100%";
            return;
        }

        characterLevelText.text = "Lv." + currentCharacter.level.ToString();

        var currentLevelData = GetCurrentLevelData_Character(currentCharacter);
        if (currentLevelData == null)
        {
            characterExpBarImage.fillAmount = 0;
        }
        else
        {
            characterExpBarImage.fillAmount = (float)currentCharacter.exp / (float)currentLevelData.exp;
        }
        characterExpPerText.text = Mathf.FloorToInt(characterExpBarImage.fillAmount * 100) + "%";

    }

    DataBase.LevelData GetCurrentLevelData_Player(int playerLevel)
    {
        foreach (var levelData in CSVManager.Instance.PlayerLevelDatas)
        {
            if (levelData.level == playerLevel) return levelData;
        }
        return null;
    }

    DataBase.LevelData GetCurrentLevelData_Character(SaveDataObjects.Character currentCharacter)
    {
        if (currentCharacter == null) return null;
        foreach (var levelData in CSVManager.Instance.CharacterLevelDatas)
        {
            if (levelData.level == currentCharacter.level) return levelData;
        }
        return null;
    }

    DataBase.Character GetCharacterData(int characterId)
    {
        return CSVManager.Instance.Characters.FirstOrDefault(character => character.id == characterId);
    }

    void OnClickChargingScreenButton()
    {
        ScreenManager.Instance.Get<ChargingScreen>().Open();
    }

    void OnClickOpenMenuScreenButton()
    {
        ScreenManager.Instance.Get<MenuScreen>().Open();
    }
}
