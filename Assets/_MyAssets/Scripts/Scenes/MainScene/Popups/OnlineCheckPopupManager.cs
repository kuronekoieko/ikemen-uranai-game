using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class OnlineCheckPopupManager
{

    public static async UniTask<API.Result.Status> CheckOnline()
    {
        Debug.Log("オンラインチェック 開始");
        var result = await OnlineChecker.IsOnline();

        //result.status = API.Result.Status.Error;
        switch (result.status)
        {
            case API.Result.Status.Success:
                break;
            case API.Result.Status.Error:
                bool isRetry = await PopupManager.Instance.GetCommonPopup().ShowAsync(1);
                if (isRetry == false) return API.Result.Status.Canceled;
                break;
            case API.Result.Status.Canceled:
                break;
            default:
                break;
        }
        Debug.Log("オンラインチェック 終了");


        return result.status;
    }

    public static async UniTask<bool> WaitUntilOnline()
    {
        var status = await CheckOnline();

        if (status == API.Result.Status.Success) return true;

        PopupManager.Instance.GetPopup<LoadingPopup>().Open();

        while (status == API.Result.Status.Error)
        {
            await UniTaskUtils.DelaySecond(0.5f);
            status = await CheckOnline();
        }
        PopupManager.Instance.GetPopup<LoadingPopup>().Close();

        return status == API.Result.Status.Canceled;
    }

}
