using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;
using Cysharp.Threading.Tasks;
using System;
using Newtonsoft.Json;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Reflection;

public class CreateHomeTexts
{

    static HomeText[] HomeTexts;
    static Date[] Dates;
    static Day[] Days;
    static DataBase.Time[] Times;

    [MenuItem("MyTool/Home Text/Create Nani")]
    static async void CreateNani()
    {
        Debug.Log("計算開始");

        // HomeTexts = await CSVManager.DeserializeAsync<HomeText>("Home/HomeTexts");
        // Dates = await CSVManager.DeserializeAsync<Date>("Home/Dates");
        // Days = await CSVManager.DeserializeAsync<Day>("Home/Days");
        // Times = await CSVManager.DeserializeAsync<DataBase.Time>("Home/Times");
        int chara_id = 1;
        string charaKey = "chara" + chara_id.ToString("D3");
        var charaTexts = await CSVManager.DeserializeAsync<CharaText>("Home/" + charaKey);


        var groups = charaTexts.GroupBy(charaText => charaText.text_id);

        foreach (var group in groups)
        {
            var list = group.ToList();
            var charaText = list[0];
            string textKey = "text" + charaText.text_id.ToString("D3");
            await SaveToNani(charaKey, textKey, list);
        }





    }

    static async UniTask SaveToNani(string charaKey, string textKey, List<CharaText> list)
    {
        Debug.Log("書き込み開始");

        string fileName = charaKey + "-" + textKey;
        string path = Application.dataPath + @"/_MyAssets/Nani/Home/" + fileName + ".nani";
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
