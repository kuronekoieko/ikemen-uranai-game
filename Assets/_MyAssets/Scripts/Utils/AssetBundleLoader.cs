using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;

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


}
