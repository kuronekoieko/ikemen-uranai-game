using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

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
    }

    private void Update()
    {
        if (SaveData.Instance == null) return;
        SaveData saveData = SaveData.Instance;

        levelText.text = saveData.player.level.ToString();
        jemFreeText.text = saveData.jemFree.ToString();
        jemChargingText.text = saveData.jemCharging.ToString();
        dayText.text = DateTime.Now.ToString("MM d ddd");
        timeText.text = DateTime.Now.ToString("hh:mm:ss");

        characterLevelText.text = "999";// エラーのとき
        characterExpPerText.text = "100%";// エラーのとき
        foreach (var character in saveData.characters)
        {
            if (character.id != saveData.currentCharacterId) continue;
            characterLevelText.text = character.level.ToString();
            characterExpPerText.text = character.exp.ToString();// TODO: %表示
        }

    }

    void OnClickChargingScreenButton()
    {

    }
}
