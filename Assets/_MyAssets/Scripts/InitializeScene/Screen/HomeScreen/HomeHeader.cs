using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HomeHeader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI jemFreeText;
    [SerializeField] TextMeshProUGUI jemChargingText;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] TextMeshProUGUI characterLevelText;
    [SerializeField] TextMeshProUGUI characterExpPerText;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Image characterExpBarImage;
    [SerializeField] Button chargingScreenButton;


    public void OnStart()
    {
        chargingScreenButton.onClick.AddListener(OnClickChargingScreenButton);
        Initialize.Instance.OnUpdate += OnUpdate;
    }

    private void OnUpdate()
    {
        // セーブデータが別のインスタンスをつくってしまうから？
        // セーブデータがロードされないため
        SaveData saveData = SaveData.Instance;

        levelText.text = saveData.player.level.ToString();
        jemFreeText.text = saveData.jemFree.ToString();
        jemChargingText.text = saveData.jemCharging.ToString();
        dayText.text = DateTime.Now.ToString("MM d ddd");
        timeText.text = DateTime.Now.ToString("hh:mm:ss");


        var currentCharacter = GetCurrentCharacter();
        if (currentCharacter == null)
        {
            characterLevelText.text = "999";
            characterExpPerText.text = "100%";
        }
        else
        {
            characterLevelText.text = currentCharacter.level.ToString();

            var currentLevelData = GetCurrentLevelData(currentCharacter);
            if (currentLevelData == null)
            {
                characterExpBarImage.fillAmount = 0;
            }
            else
            {
                characterExpBarImage.fillAmount = (float)currentCharacter.exp / (float)currentLevelData.exp;
            }
            characterExpPerText.text = (characterExpBarImage.fillAmount * 100) + "%";
        }


        var characterData = GetCharacterData(saveData.currentCharacterId);
        if (characterData == null)
        {
            characterNameText.text = "キャラクター";
        }
        else
        {
            characterNameText.text = characterData.name_jp;
        }

    }

    SaveDataObjects.Character GetCurrentCharacter()
    {
        foreach (var character in SaveData.Instance.characters)
        {
            if (character.id == SaveData.Instance.currentCharacterId) return character;
        }
        return null;
    }

    DataBase.LevelData GetCurrentLevelData(SaveDataObjects.Character currentCharacter)
    {
        if (currentCharacter == null) return null;
        foreach (var levelData in CSVManager.Instance.LevelDatas)
        {
            if (levelData.level == currentCharacter.level) return levelData;
        }
        return null;
    }

    DataBase.Character GetCharacterData(string characterId)
    {
        if (string.IsNullOrEmpty(characterId)) return null;

        foreach (var character in CSVManager.Instance.Characters)
        {
            if (character.id == characterId) return character;
        }
        return null;
    }

    void OnClickChargingScreenButton()
    {

    }
}
