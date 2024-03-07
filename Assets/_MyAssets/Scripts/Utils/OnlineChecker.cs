using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

//インターネット接続確認
public static class OnlineChecker
{
    static CancellationTokenSource _tokenSource;

    //テストアクセスするURL
    private static readonly string CHECK_TARGET_URL = "https://httpbin.org";
    // private static readonly string CHECK_TARGET_URL = "";

    public async static UniTask<Result> IsOnline(int timeOutSec = 3)
    {
        Result result = new()
        {
            status = Result.Status.Error,
            errorLog = Application.internetReachability.ToString(),
            responseJson = "",
        };

        if (Application.internetReachability == NetworkReachability.NotReachable) return result;

        // https://kan-kikuchi.hatenablog.com/entry/InternetAccessChecker
        var request = new UnityWebRequest(CHECK_TARGET_URL) { timeout = timeOutSec };


        try
        {
            var taskResult = await request.SendWebRequest().ToUniTask();
            if (taskResult.result == UnityWebRequest.Result.Success)
            {
                result.status = Result.Status.Success;
            }
            else
            {
                result.status = Result.Status.Error;
            }
            Debug.Log($"オンラインチェック {result.status}");
        }
        catch (OperationCanceledException o)
        {
            result.status = Result.Status.Canceled;
            result.errorLog = o.ToString();
            Debug.LogWarning($"オンラインチェック {result.status} {o}");
        }
        catch (Exception e)
        {

            //onError(e.ToString());

            result.status = Result.Status.Error;
            result.errorLog = e.ToString();
            Debug.LogWarning($"オンラインチェック {result.status} {e}");
        }

        //result.responseJson = request.downloadHandler.text;
        await UniTask.WaitUntil(() => request.isDone);
        request.Dispose();

        return result;
    }

    public static void CancelRequest()
    {
        if (_tokenSource == null) return;
        _tokenSource.Cancel();
    }

    public struct Result
    {
        public Status status;
        public string errorLog;
        public string responseJson;

        public enum Status
        {
            Success,
            Canceled,
            Error
        }
    }
}