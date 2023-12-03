using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using Cysharp.Threading.Tasks;

public class NaninovelTest : MonoBehaviour
{
    async void Start()
    {
        // https://naninovel.com/ja/guide/integration-options#%E6%89%8B%E5%8B%95%E5%88%9D%E6%9C%9F%E5%8C%96
        await RuntimeInitializer.InitializeAsync();
        var player = Engine.GetService<IScriptPlayer>();
        await player.PreloadAndPlayAsync("NewScript 1");
        // ツールバー Naninovel -> Resources -> Scripts でスクリプト割当
    }

    void Update()
    {

    }
}
