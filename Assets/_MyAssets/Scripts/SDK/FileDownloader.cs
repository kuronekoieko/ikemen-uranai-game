using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataBase;
using UnityEngine;

public class FileDownloader
{
    static readonly Dictionary<string, Fortune[]> fortunesDic = new();

    static readonly Dictionary<string, AudioClip> audioClipDic = new();


    public static async UniTask<Fortune[]> GetFortunes(DateTime dateTime)
    {
        string fileName = GetFortunesFileName(dateTime);
        bool success = fortunesDic.TryGetValue(fileName, out Fortune[] fortunes);
        if (success == false)
        {
            fortunes = await DownloadFortune(dateTime);
        }
        return fortunes;
    }

    static string GetFortunesFileName(DateTime dateTime)
    {
        string fileName = "Fortunes/" + dateTime.ToStringDate() + ".csv";
        return fileName;
    }

    public static async UniTask<Fortune[]> DownloadFortune(DateTime dateTime)
    {
        string fileName = GetFortunesFileName(dateTime);
        Uri uri = await FirebaseStorageManager.Instance.GetURI(fileName);
        string csv = await FirebaseStorageManager.Instance.DownloadCsvFile(uri);
        var fortunes = CSVSerializer.Deserialize<Fortune>(csv);

        if (fortunesDic.ContainsKey(fileName) == false)
        {
            fortunesDic[fileName] = fortunes;
        }

        return fortunes;
    }

    public static async UniTask<AudioClip> GetAudioClip(string path)
    {
        bool success = audioClipDic.TryGetValue(path, out AudioClip audioClip);
        if (success == false)
        {
            audioClip = await DownloadAudioClip(path);
        }
        return audioClip;
    }

    public static async UniTask<AudioClip> DownloadAudioClip(string path)
    {
        Uri uri = await FirebaseStorageManager.Instance.GetURI(path);
        var audioClip = await FirebaseStorageManager.Instance.DownloadAudio(uri);

        if (audioClipDic.ContainsKey(path) == false)
        {
            audioClipDic[path] = audioClip;
        }
        return audioClip;
    }
}
