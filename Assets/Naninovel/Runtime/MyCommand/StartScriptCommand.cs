using System;
using System.Collections;
using System.Collections.Generic;
using Naninovel;
using UnityEngine;

[CommandAlias("startScript")] // このエイリアス名でスクリプトからコマンドを呼び出す(@endScript)
public class StartScriptCommand : Command
{
    public static event Action<string> OnScriptStarted;
    // static bool isRunningNaniNovelScript;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        // IScriptPlayerサービスを取得
        var scriptPlayer = Engine.GetService<IScriptPlayer>();

        // 現在再生中のスクリプトの名前を取得
        string currentScriptName = scriptPlayer.PlayedScript.Name;

        Debug.Log($"NaniNovelスクリプト '{currentScriptName}' が開始しました。");

        //NaniNovel関連オブジェクトを非アクティブ化
        // ActivateNaniNovelObjects(false);

        // イベントをトリガーして、スクリプトが終了したことを通知
        OnScriptStarted?.Invoke(currentScriptName);

        // NaniNovelスクリプトが実行中でないことにする（後述）
        // isRunningNaniNovelScript = false;

        return UniTask.CompletedTask;
    }
}
