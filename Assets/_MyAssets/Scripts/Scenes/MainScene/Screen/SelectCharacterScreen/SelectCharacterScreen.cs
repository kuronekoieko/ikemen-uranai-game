using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SnapScroll;
using System.Linq;
using System;
using TMPro;

public class SelectCharacterScreen : BaseScreen
{
    [SerializeField] Button closeButton;
    [SerializeField] SnapScrollView snapScrollView;
    [SerializeField] CharacterSelectPool characterSelectPool;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button selectButton;
    [SerializeField] TextMeshProUGUI numberText;



    public override void OnStart()
    {
        base.OnStart();
        closeButton.onClick.AddListener(Close);
        characterSelectPool.OnStart();
        characterSelectPool.Show(CSVManager.Instance.Characters.OrderBy(character => character.id).ToArray());

        // https://tempura-kingdom.jp/snapscroll/
        snapScrollView.PageSize = Screen.width;
        var character = CSVManager.Instance.Characters.FirstOrDefault(character => character.id == SaveDataManager.SaveData.currentCharacterId);
        snapScrollView.Page = Array.IndexOf(CSVManager.Instance.Characters, character);
        snapScrollView.MaxPage = CSVManager.Instance.Characters.Length - 1;
        snapScrollView.OnPageChanged += OnPageChanged;
        snapScrollView.RefreshPage();

        leftButton.onClick.AddListener(() =>
        {
            snapScrollView.Page = Mathf.Clamp(snapScrollView.Page - 1, 0, snapScrollView.MaxPage);
            snapScrollView.RefreshPage();
        });
        rightButton.onClick.AddListener(() =>
        {
            snapScrollView.Page = Mathf.Clamp(snapScrollView.Page + 1, 0, snapScrollView.MaxPage);
            snapScrollView.RefreshPage();
        });

        selectButton.onClick.AddListener(() =>
        {
            SaveDataManager.SaveData.currentCharacterId = snapScrollView.Page + 1;
            SaveDataManager.Save();
            // キャラ切り替え
            Close();
        });
    }

    void OnPageChanged()
    {
        numberText.text = (snapScrollView.Page + 1) + "/" + (snapScrollView.MaxPage + 1);
    }

    public override void Open()
    {
        base.Open();

        snapScrollView.Page = SaveDataManager.SaveData.currentCharacterId - 1;
        snapScrollView.RefreshPage();
        Debug.Log(snapScrollView.Page);
    }

    public override void Close()
    {
        base.Close();
    }
}
