using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CommonPopup : BasePopup
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI pButtonText;
    [SerializeField] TextMeshProUGUI nButtonText;
    [SerializeField] Animator animator;


    public async UniTask<bool> ShowAsync(DataBase.PopupText popupText)
    {
        popupText ??= DataBase.PopupText.CreateDefault();
        return await ShowAsync("", popupText.text, popupText.button_positive, popupText.button_negative);
    }


    public async UniTask<bool> ShowAsync(int id)
    {
        var popupText = CSVManager.GetPopupText(id);

        return await ShowAsync(
            title: "",
            message: popupText.text,
            positive: popupText.button_positive,
            negative: popupText.button_negative);
    }


    async UniTask<bool> ShowAsync(
        string title,
        string message,
        string positive,
        string negative = "")
    {
        titleText.text = title;
        messageText.text = message;
        pButtonText.text = positive;
        nButtonText.text = negative;
        if (positiveButton) positiveButton.gameObject.SetActive(!string.IsNullOrEmpty(positive));
        if (negativeButton) negativeButton.gameObject.SetActive(!string.IsNullOrEmpty(negative));

        int status = 0;
        positiveButton.onPointerDown = async () =>
        {
            await animator.PlayAsync("ButtonDown_Positive", 1);
        };
        positiveButton.onPointerUp = async () =>
        {
            await animator.PlayAsync("ButtonUp_Positive", 1);
            status = 1;
        };
        negativeButton.onPointerDown = async () =>
        {
            await animator.PlayAsync("ButtonDown_Negative", 1);
        };
        negativeButton.onPointerUp = async () =>
        {
            await animator.PlayAsync("ButtonUp_Negative", 1);
            status = 2;
        };

        base.Open();


        await UniTask.WaitWhile(() => status == 0);
        var isSelectedPositive = status == 1;

        await animator.PlayAsync("CloseWindow");
        base.OnClose();
        return isSelectedPositive;
    }

    protected override void OnClose()
    {

    }
}
