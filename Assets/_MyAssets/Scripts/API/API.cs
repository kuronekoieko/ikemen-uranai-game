using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class API
{
    static CancellationTokenSource _tokenSource;

    public static async UniTask<Result> Get(string url, Dictionary<string, string> header = null, float timeoutSec = 10)
    {
        // POSTリクエストを送信
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.downloadHandler = new DownloadHandlerBuffer();

        if (header != null)
        {
            foreach (var pair in header)
            {
                request.SetRequestHeader(pair.Key, pair.Value);
            }
        }

        return await SendRequest(request, timeoutSec);
    }

    public async static UniTask<Result> SendRequest(UnityWebRequest request, float timeoutSec = 10)
    {
        _tokenSource = new CancellationTokenSource();
        Result result = new();

        try
        {
            (bool isTimeout, UnityWebRequest taskResult) = await request.SendWebRequest().ToUniTask().TimeOutSeconds(timeoutSec);

            if (isTimeout)
            {
                result.status = Result.Status.Canceled;
                result.errorLog = "タイムアウト";
                Debug.LogWarning($"APIリクエスト {result.status} タイムアウト");
                return result;
            }

            if (taskResult.result == UnityWebRequest.Result.Success)
            {
                result.status = Result.Status.Success;

                if (request.downloadHandler != null)
                {
                    result.responseJson = request.downloadHandler.text;
                }

            }
            else
            {
                result.status = Result.Status.Error;
            }
            Debug.Log($"APIリクエスト {result.status}");
        }
        catch (OperationCanceledException o)
        {
            result.status = Result.Status.Canceled;
            result.errorLog = o.ToString();
            Debug.LogWarning($"APIリクエスト {result.status} {o}");
        }
        catch (Exception e)
        {

            //onError(e.ToString());

            result.status = Result.Status.Error;
            result.errorLog = e.ToString();
            Debug.LogWarning($"APIリクエスト {result.status} {e}");
        }

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
