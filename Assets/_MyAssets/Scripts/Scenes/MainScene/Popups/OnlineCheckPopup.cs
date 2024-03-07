using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

public class OnlineCheckPopup : CommonPopup
{

    public async UniTask<bool> CheckOnline()
    {
        Debug.Log("オンラインチェック 開始");
        var result = await OnlineChecker.IsOnline();
        bool isOnline = false;

        switch (result.status)
        {
            case OnlineChecker.Result.Status.Success:
                isOnline = true;
                break;
            case OnlineChecker.Result.Status.Error:
                Show(
                    "",
                    "インターネットに接続されていません",
                    "OK"
                );
                break;
            case OnlineChecker.Result.Status.Canceled:
                break;
            default:
                break;
        }
        Debug.Log("オンラインチェック 終了");


        return isOnline;
    }
}
