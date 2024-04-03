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

    /*    public void Show(
            string title,
            string message,
            string positive = "",
            string negative = "",
            Action onClickPositiveButton = null,
            Action onClickNegativeButton = null)
        {
            base.Open().Forget();
            if (onClickPositiveButton != null) base.onClickPositiveButton = onClickPositiveButton;
            if (onClickNegativeButton != null) base.onClickNegativeButton = onClickNegativeButton;
            titleText.text = title;
            messageText.text = message;
            pButtonText.text = positive;
            nButtonText.text = negative;
            positiveButton.gameObject.SetActive(!string.IsNullOrEmpty(positive));
            negativeButton.gameObject.SetActive(!string.IsNullOrEmpty(negative));
        }*/


    public async UniTask<bool> ShowAsync(
        string title,
        string message,
        string positive,
        string negative = "")
    {
        titleText.text = title;
        messageText.text = message;
        pButtonText.text = positive;
        nButtonText.text = negative;
        positiveButton.gameObject.SetActive(!string.IsNullOrEmpty(positive));
        negativeButton.gameObject.SetActive(!string.IsNullOrEmpty(negative));

        // UnityEventを変換
        //  var positiveButtonEvent = positiveButton.onClick.GetAsyncEventHandler(CancellationToken.None);
        // var negativeButtonEvent = negativeButton.onClick.GetAsyncEventHandler(CancellationToken.None);

        // ボタンの入力待ち
        // UniTask pBtn = positiveButtonEvent.OnInvokeAsync();
        // UniTask nBtn = negativeButtonEvent.OnInvokeAsync();
        int status = 0;
        positiveButton.onPointerDown = async () =>
        {
            await animator.PlayAsync("ButtonDown_Positive");
        };
        positiveButton.onPointerUp = async () =>
        {
            await animator.PlayAsync("ButtonUp_Positive");
            status = 1;
        };
        negativeButton.onPointerDown = async () =>
        {
            await animator.PlayAsync("ButtonDown_Negative");
        };
        negativeButton.onPointerUp = async () =>
        {
            await animator.PlayAsync("ButtonUp_Negative");
            status = 2;
        };
        await base.Open();

        await UniTask.WaitWhile(() => status == 0);
        var isSelectedPositive = status == 1;

        await animator.PlayAsync("CloseWindow");
        await Close();
        // await UniTask.WaitUntil(() => isClosed);
        return isSelectedPositive;
    }

    protected async override UniTask OnClose()
    {
        await UniTask.DelayFrame(0);
    }
}
