using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class OnlineCheckPopup : CommonPopup
{

    public async UniTask<bool> CheckOnline()
    {
        Debug.Log("オンラインチェック 開始");
        var result = await OnlineChecker.IsOnline();
        bool isOnline = false;

        switch (result.status)
        {
            case API.Result.Status.Success:
                isOnline = true;
                break;
            case API.Result.Status.Error:
                await ShowAsync(
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

    public async UniTask<bool> CheckOnlineUntilOnline()
    {
        bool isOnline = await CheckOnline();

        if (isOnline) return isOnline;

        await PopupManager.Instance.GetPopup<LoadingPopup>().Open();

        while (isOnline == false)
        {
            await UniTaskUtils.DelaySecond(0.5f);
            isOnline = await CheckOnline();
        }
        await PopupManager.Instance.GetPopup<LoadingPopup>().Close();

        return isOnline;

    }

    public async void StartCheckOnlineLoop()
    {
        while (true)
        {
            await CheckOnlineUntilOnline();
            await UniTaskUtils.DelaySecond(3);
        }
    }

}
