using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetBundleLoader
{
    public static async UniTask<T> LoadAddressablesAsync<T>(string address) where T : UnityEngine.Object
    {
        Addressables.WebRequestOverride = EditWebRequestURL;
        Debug.Log("ロード開始 " + address);
        T asset = null;
        bool exists = await ExistsAsync(address);
        if (exists)
        {
            asset = await Addressables.LoadAssetAsync<T>(address).Task;
            Debug.Log("ロード終了 " + address);
        }
        else
        {
            Debug.LogWarning("ロード失敗 " + address);
        }
        return asset;
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

    public static string GetAudioFileName(SaveDataObjects.Character character, DataBase.Fortune fortune)
    {
        if (character == null) return "";
        // Voices/chara0001-rank04-msg14.wav
        string fileName = "Voices/" + character.IdToKey() + "-rank" + fortune.rank.ToString("D3") + "-msg" + fortune.msg_id.ToString("D3") + ".wav";
        return fileName;
    }

    /// <summary>
    /// 指定されたアドレスに紐づくアセットが存在する場合 true を返します
    /// </summary>
    static async UniTask<bool> ExistsAsync(object key)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(key).Task;

        return locations != null && locations.Count > 0;
    }
}
