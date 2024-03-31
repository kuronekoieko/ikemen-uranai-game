using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

//インターネット接続確認
public static class OnlineChecker
{

    //テストアクセスするURL
    private static readonly string CHECK_TARGET_URL = "https://httpbin.org";
    // private static readonly string CHECK_TARGET_URL = "";

    public async static UniTask<API.Result> IsOnline()
    {
        API.Result result = new()
        {
            status = API.Result.Status.Error,
            errorLog = Application.internetReachability.ToString(),
            responseJson = "",
        };

        if (Application.internetReachability == NetworkReachability.NotReachable) return result;
        var request = new UnityWebRequest(CHECK_TARGET_URL) { timeout = 3 };
        result = await API.SendRequest(request);

        return result;
    }
}