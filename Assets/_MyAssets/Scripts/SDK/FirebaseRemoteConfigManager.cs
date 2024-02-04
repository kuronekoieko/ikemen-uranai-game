using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using UnityEngine;
using Firebase.RemoteConfig;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

// https://firebase.google.com/docs/remote-config/get-started?hl=ja&authuser=0&platform=unity
public static class FirebaseRemoteConfigManager
{
  static bool IsInitialized = false;

  public static async UniTask InitializeAsync()
  {
    var defaults = new Dictionary<string, object>();
    defaults.Add(Key.google_calender_api_key, "");
    IsInitialized = false;
    await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
    await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
    await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
    IsInitialized = true;
  }


  public static async UniTask<string> GetString(string key)
  {
    await UniTask.WaitUntil(() => IsInitialized).Timeout(new TimeSpan(0, 0, 10));
    return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
  }

  public static class Key
  {
    public static string google_calender_api_key = "google_calender_api_key";
  }
}
