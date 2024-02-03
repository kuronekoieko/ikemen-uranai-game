using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;
using Cysharp.Threading.Tasks;
using UnityEditor;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class CreateHomeTexts
{
    [MenuItem("MyTool/Home Text/Test")]
    static async void Test()
    {
        Debug.Log("計算開始");
        await CSVManager.InitializeAsync();

        foreach (var homeText in CSVManager.HomeTexts)
        {
            homeText.date = CSVManager.Dates.FirstOrDefault(date => date.date_id == homeText.date_id);
            if (homeText.date == null)
            {
                Debug.LogError($"id:{homeText.id} date が存在しません");
            }
            homeText.day = CSVManager.Days.FirstOrDefault(date => date.day_id == homeText.day_id);
            if (homeText.date == null)
            {
                Debug.LogError($"id:{homeText.id} day が存在しません");
            }
            homeText.time = CSVManager.Times.FirstOrDefault(date => date.time_id == homeText.time_id);
            if (homeText.date == null)
            {
                Debug.LogError($"id:{homeText.id} time が存在しません");
            }

           // DebugUtils.LogJson(homeText);
        }


    }


    [MenuItem("MyTool/Home Text/Create Nani")]
    static async void CreateNani()
    {
        Debug.Log("計算開始");
        int chara_id = 0;

        while (true)
        {
            chara_id += 1;
            string charaKey = "chara" + chara_id.ToString("D3");
            var charaTexts = await CSVManager.DeserializeAsync<CharaText>("Home/" + charaKey);
            if (charaTexts == null) break;

            var groups = charaTexts.GroupBy(charaText => charaText.text_id);

            foreach (var group in groups)
            {
                var list = group.ToList();
                var charaText = list[0];
                string textKey = "text" + charaText.text_id.ToString("D3");
                await SaveToNani(charaKey, textKey, list);
            }
        }





        Debug.Log("終了");

    }

    static async UniTask SaveToNani(string charaKey, string textKey, List<CharaText> list)
    {
        Debug.Log("書き込み開始");

        string fileName = charaKey + "-" + textKey;
        string path = Application.dataPath + @"/_MyAssets/Naninovel/Scripts/Home/" + fileName + ".nani";
        // Debug.Log(path);
        using StreamWriter sw = File.CreateText(path);

        // 念のため並び替え
        list.OrderBy(charaText => charaText.line);

        foreach (var charaText in list)
        {
            if (!charaText.animation_id.Contains("-") && !string.IsNullOrEmpty(charaText.animation_id))
            {
                sw.WriteLine("@char " + charaKey + "." + charaText.animation_id);
            }
            if (!charaText.voice_id.Contains("-") && !string.IsNullOrEmpty(charaText.voice_id))
            {
                // string voicePath = "Voices/" + charaKey + "-" + charaText.voice_id + ".wav";
                string voicePath = AssetBundleLoader.GetShortVoiceAddress(charaKey, charaText.voice_id);
                sw.WriteLine("@voice " + voicePath);
            }
            sw.WriteLine(charaText.text);
            sw.WriteLine("");
        }



        AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);
        await UniTask.DelayFrame(1);
    }


}
