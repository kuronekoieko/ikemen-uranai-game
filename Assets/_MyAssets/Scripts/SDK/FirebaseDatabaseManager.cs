using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.Events;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using UniRx;



public static class FirebaseDatabaseManager
{
    static DatabaseReference reference;

    public static void Initialize()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }


    public static async UniTask<bool> SendSaveData(SaveData saveData)
    {
        //await PopupManager.Instance.GetPopup<OnlineCheckPopup>().CheckOnlineUntilOnline();

        string userId = FirebaseAuthenticationManager.User.UserId;
       // Debug.Log("SendSaveData " + userId);

        // 空のidを送ると、サーバーのデータ全部消える
        if (string.IsNullOrEmpty(userId)) return false;
        //Debug.Log("SendSaveData " + userId);

        string json = JsonConvert.SerializeObject(saveData);
        bool isTimeout = await reference.Child("users").Child(userId).SetRawJsonValueAsync(json).AsUniTask().TimeOutSeconds(3);
        if (isTimeout)
        {
            Debug.LogWarning("SendSaveData isTimeout");
            return false;
        }
        return true;
    }

    public static async UniTask RemoveSaveData()
    {
        //await PopupManager.Instance.GetPopup<OnlineCheckPopup>().CheckOnlineUntilOnline();

        string userId = FirebaseAuthenticationManager.User.UserId;
        // 空のidを送ると、サーバーのデータ全部消える
        if (string.IsNullOrEmpty(userId)) return;
        bool isTimeout = await reference.Child("users").Child(userId).RemoveValueAsync().AsUniTask().TimeOutSeconds(3);
        if (isTimeout)
        {
            Debug.LogWarning("RemoveSaveData isTimeout");
            return;
        }
    }


    public static async UniTask<SaveData> GetUserData(string userId)
    {

        //await PopupManager.Instance.GetPopup<OnlineCheckPopup>().CheckOnlineUntilOnline();


        // ネットワークにつながってないときは、キャッシュされてるのを取ってきてるっぽい
        // →取れるときと取れないときがある
        (bool isTimeout, DataSnapshot snapshot) = await reference.Child("users").Child(userId).GetValueAsync().AsUniTask().TimeOutSeconds(3);

        if (isTimeout)
        {
            Debug.LogWarning("GetUserData isTimeout");
            return null;
        }

        string json = snapshot.GetRawJsonValue();
        DebugUtils.LogJson(json);
        if (string.IsNullOrEmpty(json)) return null;
        SaveData saveData = null;
        try
        {
            saveData = JsonConvert.DeserializeObject<SaveData>(json);
            DebugUtils.LogJson(saveData);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        return saveData;
    }

    public static async Task<List<SaveData>> GetUsers()
    {
        //await PopupManager.Instance.GetPopup<OnlineCheckPopup>().CheckOnlineUntilOnline();

        List<SaveData> saveDatas = new();

        // ネットワークにつながってないときは、キャッシュされてるのを取ってきてるっぽい
        // オフラインの場合、ユーザーIDがかぶる可能性あり
        (bool isTimeout, DataSnapshot snapshot) = await reference.Child("users").GetValueAsync().AsUniTask().TimeOutSeconds(3);
        if (isTimeout)
        {
            Debug.LogWarning("GetUsers() isTimeout");
            return null;
        }


        // https://www.project-unknown.jp/entry/firebase-login-vol3_1#DataSnapshot-snapshot--taskResult
        IEnumerator<DataSnapshot> result = snapshot.Children.GetEnumerator();

        while (result.MoveNext())
        {
            DataSnapshot data = result.Current;
            string json = data.GetRawJsonValue();
            if (string.IsNullOrEmpty(json)) continue;
            SaveData saveData;
            try
            {
                saveData = JsonConvert.DeserializeObject<SaveData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                continue;
            }
            saveDatas.Add(saveData);
        }
        return saveDatas;
    }


    public static async Task<Dictionary<string, SaveData>> GetUsersDic()
    {
        Dictionary<string, SaveData> saveDatas = new();

        DataSnapshot snapshot = await reference.Child("users").GetValueAsync();

        // https://www.project-unknown.jp/entry/firebase-login-vol3_1#DataSnapshot-snapshot--taskResult
        IEnumerator<DataSnapshot> result = snapshot.Children.GetEnumerator();

        while (result.MoveNext())
        {
            DataSnapshot data = result.Current;
            string json = data.GetRawJsonValue();
            if (string.IsNullOrEmpty(json)) continue;
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
            saveDatas[saveData.firebaseUserId] = saveData;
        }

        return saveDatas;
    }


    static void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
    }

}
