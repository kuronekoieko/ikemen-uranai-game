using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class OnlineCheckPopupManager
{

    public static async UniTask<bool> CheckOnline()
    {
        Debug.Log("オンラインチェック 開始");
        var result = await OnlineChecker.IsOnline();
        bool isOnline = false;
        // result.status = API.Result.Status.Error;
        switch (result.status)
        {
            case API.Result.Status.Success:
                isOnline = true;
                break;
            case API.Result.Status.Error:
                await PopupManager.Instance.GetCommonPopup().ShowAsync(
                    "",
                    "インターネットに接続されていません",
                    "OK"
                );
                break;
            case API.Result.Status.Canceled:
                break;
            default:
                break;
        }
        Debug.Log("オンラインチェック 終了");


        return isOnline;
    }

    public static async UniTask<bool> CheckOnlineUntilOnline()
    {
        bool isOnline = await CheckOnline();

        if (isOnline) return isOnline;

        PopupManager.Instance.GetPopup<LoadingPopup>().Open();

        while (isOnline == false)
        {
            await UniTaskUtils.DelaySecond(0.5f);
            isOnline = await CheckOnline();
        }
        PopupManager.Instance.GetPopup<LoadingPopup>().Close();

        return isOnline;

    }

    public static async void StartCheckOnlineLoop()
    {
        while (true)
        {
            await CheckOnlineUntilOnline();
            await UniTaskUtils.DelaySecond(3);
        }
    }

}
