using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;

//インターネット接続確認
public static class OnlineChecker
{

    //テストアクセスするURL
    // private static readonly string CHECK_TARGET_URL = "https://httpbin.org";
    private static readonly string CHECK_TARGET_URL = "https://www.google.com/?hl=ja";

    public async static UniTask<bool> IsOnline(Action<string> onError, float timeOutSec = 3)
    {

        // if (Application.internetReachability == NetworkReachability.NotReachable) return false;

        bool bo = false;

        var request = new UnityWebRequest(CHECK_TARGET_URL);
        var task1 = request.SendWebRequest().ToUniTask();
        var task2 = UniTaskUtils.DelaySecond(timeOutSec);

        try
        {
            // Debug.Log($"アクセス-開始 {CHECK_TARGET_URL}");

            var result = await UniTask.WhenAny(task1, task2);

            if (result.result.result == UnityWebRequest.Result.Success)
            {
                // Debug.Log($"アクセス-成功 {CHECK_TARGET_URL}");
                bo = true;
            }
        }
        catch (Exception e)
        {
            // Debug.LogWarning($"例外-失敗 {e}");
            onError(e.ToString());
        }
        finally
        {
            await UniTask.WaitUntil(() => request.isDone);
            request.Dispose();
            // Debug.Log($"アクセス-廃棄");
        }

        return bo;
    }
}