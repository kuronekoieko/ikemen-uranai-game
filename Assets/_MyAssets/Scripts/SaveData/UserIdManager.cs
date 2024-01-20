using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using UniRx;

public static class UserIdManager
{

    // intの最大は21億ちょい
    // 9億9999万9999ができる9桁が実質最大
    // 1スタートにしたいので、最初のuidは1億0000万0000
    // 1桁目はサーバーで変えてもいいかも
    // 原神のuid仕様
    // https://genshin-impact.fandom.com/ja/wiki/%E3%83%A6%E3%83%BC%E3%82%B6%E3%83%BCID

    static int DefaultUserIdInt => (int)Mathf.Pow(10, 9);
    public static string DefaultUserId => ToString(DefaultUserIdInt);

    public static async UniTask<string> CreateNewUserId()
    {
        var users = await FirebaseDatabaseManager.Instance.GetUsers();
        int maxUserNumber = users.Max(user => Parse(user.userId));
        int newUserIdInt = maxUserNumber + 1;
        return ToString(newUserIdInt);
    }

    static string ToString(int userIdInt)
    {
        return userIdInt.ToString("D9");
    }

    public static int Parse(string id)
    {
        if (int.TryParse(id, out int idInt))
        {
            return idInt;
        }
        else
        {
            Debug.LogError("user idのパースに失敗 id: " + id);
            return DefaultUserIdInt;
        }
    }

}
