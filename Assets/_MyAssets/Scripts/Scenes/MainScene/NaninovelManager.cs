using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class NaninovelManager
{
    static bool IsInitialized = false;

    public static async Cysharp.Threading.Tasks.UniTask InitializeAsync(int characterId)
    {
        Debug.Log("Naninovel 初期化開始");
        IsInitialized = false;
        // https://naninovel.com/ja/guide/integration-options#%E6%89%8B%E5%8B%95%E5%88%9D%E6%9C%9F%E5%8C%96
        await RuntimeInitializer.InitializeAsync();
        IsInitialized = true;
        Debug.Log("Naninovel 初期化終了");

        await PlayHomeAsync(characterId);
    }

    public static async Cysharp.Threading.Tasks.UniTask PlayHomeAsync(int characterId)
    {
        string scriptName = "HomeIdle" + characterId.ToString("D3");
        await PlayAsync(scriptName);
    }

    public static async Cysharp.Threading.Tasks.UniTask PlayAsync(string scriptName)
    {
        //await UniTask.WaitUntil(() => IsInitialized).Timeout(new TimeSpan(0, 0, 10));
        if (IsInitialized == false) return;

        Debug.Log("Naninovel " + scriptName + ".nani");
        var player = Engine.GetService<IScriptPlayer>();
        // ツールバー Naninovel -> Resources -> Scripts でスクリプト割当
        await player.PreloadAndPlayAsync(scriptName + ".nani");
    }
}
