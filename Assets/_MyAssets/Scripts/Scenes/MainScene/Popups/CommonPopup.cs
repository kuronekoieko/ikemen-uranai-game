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

    public void Show(
        string title,
        string message,
        string positive = "",
        string negative = "",
        Action onClickPositiveButton = null,
        Action onClickNegativeButton = null)
    {
        base.Open();
        if (onClickPositiveButton != null) base.onClickPositiveButton = onClickPositiveButton;
        if (onClickNegativeButton != null) base.onClickNegativeButton = onClickNegativeButton;
        titleText.text = title;
        messageText.text = message;
        pButtonText.text = positive;
        nButtonText.text = negative;
        positiveButton.gameObject.SetActive(!string.IsNullOrEmpty(positive));
        negativeButton.gameObject.SetActive(!string.IsNullOrEmpty(negative));
    }

    public async UniTask<bool> ShowAsync(
        string title,
        string message,
        string positive,
        string negative = "")
    {
        base.Open();

        titleText.text = title;
        messageText.text = message;
        pButtonText.text = positive;
        nButtonText.text = negative;
        positiveButton.gameObject.SetActive(!string.IsNullOrEmpty(positive));
        negativeButton.gameObject.SetActive(!string.IsNullOrEmpty(negative));

        // UnityEventを変換
        var positiveButtonEvent = positiveButton.onClick.GetAsyncEventHandler(CancellationToken.None);
        var negativeButtonEvent = negativeButton.onClick.GetAsyncEventHandler(CancellationToken.None);

        // ボタンの入力待ち
        UniTask pBtn = positiveButtonEvent.OnInvokeAsync();
        UniTask nBtn = negativeButtonEvent.OnInvokeAsync();

        var btnIndex = await UniTask.WhenAny(pBtn, nBtn);
        var isSelectedPositive = btnIndex == 0;
        return isSelectedPositive;
    }
}
