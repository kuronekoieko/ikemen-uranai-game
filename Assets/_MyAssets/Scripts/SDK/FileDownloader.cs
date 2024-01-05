using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataBase;
using UnityEngine;
using System.IO;

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
        string url = await FirebaseStorageManager.Instance.GetURI(fileName);
        string csv = await FirebaseStorageManager.Instance.DownloadCsvFile(url);
        var fortunes = CSVSerializer.Deserialize<Fortune>(csv);

        if (fortunesDic.ContainsKey(fileName) == false)
        {
            fortunesDic[fileName] = fortunes;
        }

        return fortunes;
    }

    public static async UniTask<AudioClip> GetAudioClip(string path)
    {

        audioClipDic.TryGetValue(path, out AudioClip audioClip);
        if (audioClip) return audioClip;

        // audioClip = LoadAudioFromLocal(path);
        if (audioClip)
        {
            audioClipDic[path] = audioClip;
            return audioClip;
        }

        audioClip = await DownloadAudioClip(path);
        if (audioClip)
        {
            audioClipDic[path] = audioClip;
            return audioClip;
        }

        return audioClip;
    }

    static async UniTask<AudioClip> DownloadAudioClip(string path)
    {
        string url = await FirebaseStorageManager.Instance.GetURI(path);

        if (path.Contains(".wav"))
        {
            var downloadedAudio = await FirebaseStorageManager.Instance.DownloadAudio(url);
            if (downloadedAudio == null) return null;
            SaveLocal(downloadedAudio.data, path);
            return downloadedAudio.audioClip;
        }
        else
        {
            // var assetBundleName = path.Replace("Voices/", "");
            // var audioClip = await AssetBundleLoader.DownloadAssetBundleAsync<AudioClip>(url, assetBundleName);
            //return audioClip;
        }
        return null;
    }

    static void SaveLocal(byte[] audioData, string path)
    {
        // 保存先のファイルパス
        string filePath = Application.persistentDataPath + "/" + path;
        Debug.Log(filePath);

        string directoryPath = Path.GetDirectoryName(filePath);
        Debug.Log(directoryPath);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        try
        {
            // ファイルに保存
            File.WriteAllBytes(filePath, audioData);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    static AudioClip LoadAudioFromLocal(string path)
    {
        // ファイルのフルパスを取得
        string filePath = Application.persistentDataPath + "/" + path;

        // ファイルが存在するか確認
        if (File.Exists(filePath))
        {
            // ファイルをバイト配列として読み込む
            //  byte[] audioData = File.ReadAllBytes(filePath);
            byte[] fileBytes = File.ReadAllBytes(filePath);
            DebugUtils.LogJson(fileBytes);

            Debug.Log("Audio loaded from: " + filePath);
            return WavUtility.ToAudioClip(filePath);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }
    }

    public static string GetAudioFileName(string characterId, Fortune fortune)
    {
        // Voices/chara0001-rank04-msg14.wav
        string fileName = "Voices/chara" + characterId + "-rank" + fortune.rank.ToString("D3") + "-msg" + fortune.msg_id.ToString("D3") + ".wav";
        return fileName;
    }


}
