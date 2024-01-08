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



public class FirebaseDatabaseManager : Singleton<FirebaseDatabaseManager>
{
    DatabaseReference reference;

    public void Initialize()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }


    public async UniTask SendSaveData(SaveData saveData)
    {
        string userId = FirebaseAuthenticationManager.Instance.User.UserId;
        // 空のidを送ると、サーバーのデータ全部消える
        if (string.IsNullOrEmpty(userId)) return;
        string json = JsonConvert.SerializeObject(saveData);
        await reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public async UniTask RemoveSaveData()
    {
        string userId = FirebaseAuthenticationManager.Instance.User.UserId;
        // 空のidを送ると、サーバーのデータ全部消える
        if (string.IsNullOrEmpty(userId)) return;
        await reference.Child("users").Child(userId).RemoveValueAsync();
    }


    public async UniTask<SaveData> GetUserData(string userId)
    {
        DataSnapshot snapshot = await reference.Child("users").Child(userId).GetValueAsync();
        string json = snapshot.GetRawJsonValue();
        Debug.Log(json);
        if (string.IsNullOrEmpty(json)) return null;
        SaveData saveData = null;
        try
        {
            saveData = JsonConvert.DeserializeObject<SaveData>(json);
            DebugUtils.LogJson(saveData);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return saveData;
    }

    public async Task<List<SaveData>> GetUsers()
    {
        List<SaveData> saveDatas = new();

        DataSnapshot snapshot = await reference.Child("users").GetValueAsync();

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
            catch (System.Exception e)
            {
                Debug.LogError(e);
                continue;
            }
            saveDatas.Add(saveData);
        }
        return saveDatas;
    }


    public async Task<Dictionary<string, SaveData>> GetUsersDic()
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


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
    }

}
