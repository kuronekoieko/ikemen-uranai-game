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
        var a = await Addressables.LoadAssetAsync<T>(address).Task;
        return a;
    }

    public static async UniTask<T> DownloadAddressablesAsync<T>(string address) where T : UnityEngine.Object
    {
        Debug.Log("aaaaaaaaaa");
        var a = await Addressables.DownloadDependenciesAsync(address).Task;
        Debug.Log("bbbbbbbbbbb");

        // var a = await Addressables.LoadAssetAsync<T>(address).Task;
        return a as T;
    }
}
