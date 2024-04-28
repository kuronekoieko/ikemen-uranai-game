using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SnapScroll;
using System.Linq;
using System;
using TMPro;
using Cysharp.Threading.Tasks;

public class SelectCharacterScreen : BaseScreen
{
    [SerializeField] SnapScrollView snapScrollView;
    [SerializeField] CharacterSelectPool characterSelectPool;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button selectButton;
    [SerializeField] TextMeshProUGUI numberText;


    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
        characterSelectPool.OnStart();
        characterSelectPool.Show(CSVManager.Characters.OrderBy(character => character.id).ToArray());

        // https://tempura-kingdom.jp/snapscroll/
        snapScrollView.PageSize = CanvasScaler.referenceResolution.x;
        var character = CSVManager.Characters.FirstOrDefault(character => character.id == SaveDataManager.SaveData.currentCharacterId);
        snapScrollView.Page = Array.IndexOf(CSVManager.Characters, character);
        snapScrollView.MaxPage = CSVManager.Characters.Length - 1;
        snapScrollView.OnPageChanged += OnPageChanged;
        snapScrollView.RefreshPage(false);

        leftButton.AddListener(() =>
        {
            snapScrollView.Page = Mathf.Clamp(snapScrollView.Page - 1, 0, snapScrollView.MaxPage);
            snapScrollView.RefreshPage();
            return UniTask.DelayFrame(0);
        });
        rightButton.AddListener(() =>
        {
            snapScrollView.Page = Mathf.Clamp(snapScrollView.Page + 1, 0, snapScrollView.MaxPage);
            snapScrollView.RefreshPage();
            return UniTask.DelayFrame(0);
        });

        selectButton.AddListener(OnClickSelectButton);
    }

    async UniTask OnClickSelectButton()
    {
        if (SaveDataManager.SaveData.currentCharacterId != snapScrollView.Page + 1)
        {
            SaveDataManager.SaveData.currentCharacterId = snapScrollView.Page + 1;
            await SaveDataManager.SaveAsync();

            await NaninovelManager.PlayHomeAsync(SaveDataManager.SaveData.currentCharacterId);

            PageManager.Instance.Get<HomePage>().EnableButtons(true);
        }

        await Close();
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

    public override UniTask Close()
    {
        base.Close();
        return UniTask.DelayFrame(0);
    }
}
