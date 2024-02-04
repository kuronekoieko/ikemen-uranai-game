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
    [SerializeField] SnapScrollView snapScrollView;
    [SerializeField] CharacterSelectPool characterSelectPool;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button selectButton;
    [SerializeField] TextMeshProUGUI numberText;



    public override void OnStart()
    {
        base.OnStart();
        characterSelectPool.OnStart();
        characterSelectPool.Show(CSVManager.Characters.OrderBy(character => character.id).ToArray());

        // https://tempura-kingdom.jp/snapscroll/
        snapScrollView.PageSize = CanvasManager.Instance.CanvasScaler.referenceResolution.x;
        var character = CSVManager.Characters.FirstOrDefault(character => character.id == SaveDataManager.SaveData.currentCharacterId);
        snapScrollView.Page = Array.IndexOf(CSVManager.Characters, character);
        snapScrollView.MaxPage = CSVManager.Characters.Length - 1;
        snapScrollView.OnPageChanged += OnPageChanged;
        snapScrollView.RefreshPage(false);

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

        selectButton.onClick.AddListener(async () =>
        {
            // 連打対策
            selectButton.interactable = false;


            if (SaveDataManager.SaveData.currentCharacterId != snapScrollView.Page + 1)
            {
                SaveDataManager.SaveData.currentCharacterId = snapScrollView.Page + 1;
                SaveDataManager.Save();
                await NaninovelManager.PlayHomeAsync(SaveDataManager.SaveData.currentCharacterId);
                PageManager.Instance.Get<HomePage>().EnableButtons(true);
                EndScriptCommand.OnScriptEnded += (currentScriptName) =>
                {
                    if (currentScriptName.Contains("HomeIdle"))
                    {
                        Close();
                        selectButton.interactable = true;
                    }
                };
            }
            else
            {
                Close();
                selectButton.interactable = true;
            }



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
        snapScrollView.RefreshPage(false);
    }

    public override void Close()
    {
        base.Close();
    }
}
