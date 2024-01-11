using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class NaninovelInitializer : MonoBehaviour
{
    public static async Cysharp.Threading.Tasks.UniTask InitializeAsync()
    {
        Debug.Log("Naninovel読み込み開始");

        string scriptName = "HomeIdle";
        // https://naninovel.com/ja/guide/integration-options#%E6%89%8B%E5%8B%95%E5%88%9D%E6%9C%9F%E5%8C%96
        await RuntimeInitializer.InitializeAsync();


        var player = Engine.GetService<IScriptPlayer>();
        // ツールバー Naninovel -> Resources -> Scripts でスクリプト割当
        await player.PreloadAndPlayAsync(scriptName);

        Debug.Log("Naninovel読み込み終了");
    }
}
