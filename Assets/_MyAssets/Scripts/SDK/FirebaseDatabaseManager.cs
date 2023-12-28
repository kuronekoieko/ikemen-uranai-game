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
        // 空のidを送ると、サーバーのデータ全部消える
        if (string.IsNullOrEmpty(saveData.uid)) return;
        string json = JsonUtility.ToJson(saveData);
        await reference.Child("users").Child(saveData.uid).SetRawJsonValueAsync(json);
    }

    public async UniTask<SaveData> GetUserData(string uid)
    {
        DataSnapshot snapshot = await reference.Child("users").Child(uid).GetValueAsync();
        string json = snapshot.GetRawJsonValue();
        Debug.Log(json);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json);
        return saveData;
    }


    public async Task<Dictionary<string, SaveData>> GetSaveDataAry(int count)
    {

        Dictionary<string, SaveData> saveDatas = new();

        DataSnapshot snapshot = await reference.Child("users").GetValueAsync();

        // https://www.project-unknown.jp/entry/firebase-login-vol3_1#DataSnapshot-snapshot--taskResult
        IEnumerator<DataSnapshot> result = snapshot.Children.GetEnumerator();

        while (result.MoveNext())
        {
            DataSnapshot data = result.Current;
            SaveData saveData = new SaveData();
            string json = data.GetRawJsonValue();
            JsonUtility.FromJsonOverwrite(data.GetRawJsonValue(), saveData);
            saveData = JsonConvert.DeserializeObject<SaveData>(json);
            saveDatas[saveData.uid] = saveData;
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
