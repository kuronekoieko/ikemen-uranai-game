using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

public static class UserIdManager
{

    // intの最大は21億ちょい
    // 9億9999万9999ができる9桁が実質最大
    // 1スタートにしたいので、最初のuidは1億0000万0000
    // 1桁目はサーバーで変えてもいいかも
    // 原神のuid仕様
    // https://genshin-impact.fandom.com/ja/wiki/%E3%83%A6%E3%83%BC%E3%82%B6%E3%83%BCID

    // 10の9乗は、0が9個なので10桁
    // idは9桁にしたいので、10の8乗にする
    static int DefaultUserIdInt => (int)Mathf.Pow(10, userIdLength - 1);
    public static string DefaultUserId => ToString(DefaultUserIdInt);
    static readonly int userIdLength = 9;

    public static async UniTask<string> ValidateUserId(string userId)
    {
        if (userId == DefaultUserId)
        {
            return await CreateNewUserId();
        }
        if (userId.Length != userIdLength)
        {
            return await CreateNewUserId();
        }

        return userId;
    }

    static async UniTask<string> CreateNewUserId()
    {
        var users = await FirebaseDatabaseManager.GetUsers();
        if (users == null) return null;
        int maxUserNumber = users
            .Where(user => user.userId.Length == userIdLength)
            .Max(user => Parse(user.userId));
        int newUserIdInt = maxUserNumber + 1;
        return ToString(newUserIdInt);
    }

    static string ToString(int userIdInt)
    {
        return userIdInt.ToString($"D{userIdLength}");
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
