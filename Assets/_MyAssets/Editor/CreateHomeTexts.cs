using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;
using Cysharp.Threading.Tasks;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

public class CreateHomeTexts
{

    [MenuItem("MyTool/Google Calender Test")]
    static async void GoogleCalenderTest()
    {
        Debug.Log("計算開始");

        await FirebaseRemoteConfigManager.InitializeAsync();
        var holidays = await GoogleCalendarAPI.GetHolidaysAsync(2024);

        foreach (var item in holidays)
        {
            Debug.Log(item);
        }
        Debug.Log("--------------------");

        holidays = await GoogleCalendarAPI.GetHolidaysAsync(2024);

        foreach (var item in holidays)
        {
            Debug.Log(item);
        }

        Debug.Log("--------------------");


        holidays = await GoogleCalendarAPI.GetHolidaysAsync(2025);

        foreach (var item in holidays)
        {
            Debug.Log(item);
        }
    }

    [MenuItem("MyTool/Home Text/Test")]
    static async void Test()
    {
        Debug.Log("計算開始");
        await CSVManager.InitializeAsync();
        await FirebaseRemoteConfigManager.InitializeAsync();

        DateTime dateTime = "2024/1/8 21:00".ToDateTime();
        Debug.Log(dateTime);

        var holidays = await GoogleCalendarAPI.GetHolidaysAsync(dateTime.Year);
        var homeText = HomeTextManager.GetHomeText(1, dateTime, holidays, CSVManager.HomeTexts);

        Debug.Log(homeText.FileName);
        DebugUtils.LogJson(homeText);
    }



    [MenuItem("MyTool/Home Text/Create Nani")]
    static async void CreateNani()
    {
        Debug.Log("計算開始");
        int chara_id = 0;

        await CSVManager.InitializeAsync();

        while (true)
        {
            chara_id += 1;
            var character = CSVManager.Characters.Where(character => character.id == chara_id).FirstOrDefault();

            string charaKey = "chara" + chara_id.ToString("D3");
            var charaTexts = await CSVManager.DeserializeAsync<CharaText>("Home/" + charaKey);
            if (charaTexts == null) break;

            var groups = charaTexts.GroupBy(charaText => charaText.text_id);

            foreach (var group in groups)
            {
                var list = group.ToList();
                var charaText = list[0];
                string textKey = "text" + charaText.text_id.ToString("D3");
                await SaveToNani(charaKey, textKey, list, character.name_jp);
            }
        }





        Debug.Log("終了");

    }

    static async UniTask SaveToNani(string charaKey, string textKey, List<CharaText> list, string charaName)
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
            sw.WriteLine(charaName + ": " + charaText.text);
            sw.WriteLine("");
        }

        sw.WriteLine("@printer Home visible:false");
        //sw.WriteLine("");
        sw.WriteLine("@stop");


        AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);
        await UniTask.DelayFrame(1);
    }


}
