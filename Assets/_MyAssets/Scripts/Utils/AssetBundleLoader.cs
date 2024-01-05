using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class AssetBundleLoader
{

    public static async UniTask<T> DownloadAssetBundleAsync<T>(string url, string assetBundleName) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(url)) return null;

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            return null;
        }
        else
        {
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            var obj = await assetBundle.LoadAssetAsync(assetBundleName);
            return obj as T;
        }
    }


    public static async UniTask<T> LoadAddressablesAsync<T>(string address) where T : UnityEngine.Object
    {
        Addressables.WebRequestOverride = EditWebRequestURL;
        Debug.Log("ロード開始 " + address);
        T a = await Addressables.LoadAssetAsync<T>(address).Task;
        // Debug.Log("ロード終了");
        return a;
    }

    public static async UniTask<T> DownloadAddressablesAsync<T>(string address) where T : UnityEngine.Object
    {
        Addressables.WebRequestOverride = EditWebRequestURL;
        var a = await Addressables.DownloadDependenciesAsync(address).Task;
        return a as T;
    }

    // https://gist.github.com/anmq0502/a229a048f27c91e775aeabf40517a1bd
    // firebaseはそのままダウンロードできない
    static void EditWebRequestURL(UnityWebRequest request)
    {
        if (request.url.EndsWith(".bundle", StringComparison.OrdinalIgnoreCase) || request.url.EndsWith(".json", StringComparison.OrdinalIgnoreCase) || request.url.EndsWith(".hash", StringComparison.OrdinalIgnoreCase))
        {
            request.url = request.url + "?alt=media";
            //  Debug.Log("EditWebRequestURL " + request.url);
        }
    }
}
