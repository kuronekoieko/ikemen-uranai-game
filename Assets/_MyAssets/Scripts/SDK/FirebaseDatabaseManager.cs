using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.Events;
using System.Threading.Tasks;



public class FirebaseDatabaseManager : Singleton<FirebaseDatabaseManager>
{
    DatabaseReference reference;

    public void Initialize()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task SetUserData(Uranaishi uranaishi)
    {
        // 空のidを送ると、サーバーのデータ全部消える
        if (string.IsNullOrEmpty(uranaishi.id)) return;
        string json = JsonUtility.ToJson(uranaishi);
        await reference.Child("users").Child(uranaishi.id)
        .SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                // Debug.Log("成功" + uranaishi.id);
            }
            else
            {
                // Debug.Log("失敗" + uranaishi.id);
            }
        });
        //reference.Child("users").Push().SetRawJsonValueAsync(json);
    }

    public async Task<Uranaishi> GetUserData(string uranaishiId)
    {
        Uranaishi uranaishi = null;
        await reference.Child("users").Child(uranaishiId)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.GetRawJsonValue());
                uranaishi = new Uranaishi();
                JsonUtility.FromJsonOverwrite(snapshot.GetRawJsonValue(), uranaishi);
                // Do something with snapshot...
            }
        });

        return uranaishi;
    }


    public async Task<Uranaishi[]> GetUranaishiAry(int count)
    {

        List<Uranaishi> uranaishiList = new List<Uranaishi>();


        await reference.Child("users")
        .GetValueAsync().ContinueWithOnMainThread(async task =>
        {
            await task;
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {

                // https://www.project-unknown.jp/entry/firebase-login-vol3_1#DataSnapshot-snapshot--taskResult
                DataSnapshot snapshot = task.Result;
                IEnumerator<DataSnapshot> result = snapshot.Children.GetEnumerator();

                while (result.MoveNext())
                {
                    DataSnapshot data = result.Current;
                    Uranaishi uranaishi = new Uranaishi();
                    JsonUtility.FromJsonOverwrite(data.GetRawJsonValue(), uranaishi);
                    foreach (var review in uranaishi.reviews)
                    {
                        review.uranaishi = uranaishi;
                    }
                    //uranaishi.CheckSchedules(Constant.Instance.reserveDurationMin, Constant.Instance.reserveDays);
                    uranaishi.CheckScheduleMatrix();
                    uranaishiList.Add(uranaishi);
                    // Debug.Log(data.GetRawJsonValue());

                    // Debug.Log(uranaishi.id);
                }
            }
        });

        return uranaishiList.ToArray();
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
