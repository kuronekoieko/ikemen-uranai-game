using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Google.Apis.Auth.OAuth2;



public class GoogleCloudStorage
{
    static CancellationTokenSource _tokenSource;
    static string API_KEY { get; } = "";
    static string OAUTH2_TOKEN { get; }

    static string GetAPIEndPoint(string BUCKET_NAME, string OBJECT_NAME)
    {
        return $"https://storage.googleapis.com/storage/v1/b/{BUCKET_NAME}/o/{OBJECT_NAME}?alt=media";
    }

    public static async Task DownloadByteRangeAsync(
           string bucketName = "com-ikevo-game-test",
           string objectName = "test-001.mp3",
           long firstByte = 0,
           long lastByte = 20,
           string localPath = "my-local-path/my-file-name")
    {
        var storageClient = StorageClient.Create();

        // Create an HTTP request for the media, for a limited byte range.
        StorageService storage = storageClient.Service;
        var uri = new Uri($"{storage.BaseUri}b/{bucketName}/o/{objectName}?alt=media");

        var request = new HttpRequestMessage { RequestUri = uri };
        request.Headers.Range = new RangeHeaderValue(firstByte, lastByte);

        using var outputFile = File.OpenWrite(localPath);
        // Use the HttpClient in the storage object because it supplies
        // all the authentication headers we need.
        var response = await storage.HttpClient.SendAsync(request);
        await response.Content.CopyToAsync(outputFile, null);
        Console.WriteLine($"Downloaded {objectName} to {localPath}.");
    }


    public static async UniTask<Result> DownloadAsync()
    {



        // string requestJson = JsonConvert.SerializeObject(requestData, Formatting.Indented);
        // string requestJson = JsonUtility.ToJson(requestData, true);
        // Debug.Log("リクエスト " + requestJson);

        // string apiKey = "";

        // POSTするデータ
        // byte[] data = System.Text.Encoding.UTF8.GetBytes(requestJson);
        string bucketName = "com-ikevo-game-test";
        string objectName = "test-001.mp3";
        string apiEndPint = GetAPIEndPoint(bucketName, objectName);

        // POSTリクエストを送信
        UnityWebRequest request = UnityWebRequest.Post(apiEndPint, "POST");

        // request.uploadHandler = new UploadHandlerRaw(data);
        request.downloadHandler = new DownloadHandlerBuffer();
        //request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + OAUTH2_TOKEN);

        _tokenSource = new CancellationTokenSource();
        Result result = new();

        try
        {

            await request.SendWebRequest()
            .ToUniTask(Progress.Create<float>(progress =>
            {
            }), cancellationToken: _tokenSource.Token);

            result.status = Result.Status.Success;

        }
        catch (OperationCanceledException o)
        {
            result.status = Result.Status.Canceled;
            result.errorLog = o.ToString();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            result.status = Result.Status.Error;
            result.errorLog = e.ToString();
        }
        result.responseJson = request.downloadHandler.text;
        Debug.Log("レスポンス: " + request.downloadHandler.text);

        _tokenSource = null;
        return result;
    }




    public AudioSource audioSource;


    public static async UniTask DownloadAudioFile()
    {
        string bucketName = "com-ikevo-game-test";
        string objectName = "test-001.mp3";
        var credentials = GoogleCredential.FromFile("Assets/_MyAssets/Scripts/API/shaped-epigram-393712-6ea863eb4aa3.json"); // ローカルに保存されたサービスアカウントのJSONキー ファイルへのパス
        var storage = StorageClient.Create(credentials);

        using var memoryStream = new MemoryStream();
        await storage.DownloadObjectAsync(bucketName, objectName, memoryStream);

        // メモリストリームからオーディオデータを取得
        byte[] audioData = memoryStream.ToArray();

        // UnityのAudioSourceにデータをセットして再生
        //audioSource.clip = WavUtility.ToAudioClip(audioData, 0, audioData.Length, 0, 0);
        // audioSource.Play();
    }

    public struct Result
    {
        public Status status;
        public string errorLog;
        public string responseJson;

        public enum Status
        {
            Success,
            Canceled,
            Error
        }
    }
}

