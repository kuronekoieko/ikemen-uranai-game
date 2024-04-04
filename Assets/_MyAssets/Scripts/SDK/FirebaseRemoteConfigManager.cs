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

  public static async UniTask<bool> Initialize()
  {
    var defaults = new Dictionary<string, object>();
    bool success;
    try
    {
      await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
      await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
      await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
      success = true;
    }
    catch (Exception e)
    {
      Debug.LogError(e);
      success = false;
    }
    return success;
  }


  public static string GetString(string key)
  {


    return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
  }

  public static int GetInt(string key)
  {


    return (int)FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
  }

  public static bool GetBool(string key)
  {


    return FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
  }

  public static class Key
  {
    public static string google_calender_api_key = "google_calender_api_key";
    public static string local_push_test_sec = "local_push_test_sec";
    public static string local_push_test_count = "local_push_test_count";
    public static string local_push_test_duration = "local_push_test_duration";
    public static string local_push_test_text = "local_push_test_text";
    public static string is_maintenance = "is_maintenance";
    public static string latest_version = "latest_version";
    public static string test_x = "test_x";
    public static string test_y = "test_y";
    public static string test_w = "test_w";
    public static string test_h = "test_h";
    public static string url_account_transfer = "url_account_transfer";
    public static string url_app_store_page = "url_app_store_page";
    public static string url_contact_us = "url_contact_us";
    public static string url_disclaimer = "url_disclaimer";
    public static string url_faq = "url_faq";
    public static string url_how_to_play = "url_how_to_play";
    public static string url_privacy_policy = "url_privacy_policy";
    public static string url_terms_of_use = "url_terms_of_use";
    public static string url_x = "url_x";
  }
}
