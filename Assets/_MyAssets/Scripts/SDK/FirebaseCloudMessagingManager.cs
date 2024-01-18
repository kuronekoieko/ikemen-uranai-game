using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Firebase.Messaging;
using UnityEngine.Events;

// https://firebase.google.com/docs/cloud-messaging/unity/client?hl=ja
public static class FirebaseCloudMessagingManager
{
    public static UnityAction<string> onMessageReceived;

    public static async void Initialize()
    {
        onMessageReceived = (m) => { };
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
        await CheckGooglePlay();
    }

    static void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    static void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);
        onMessageReceived(e.Message.MessageId);
    }

    static async UniTask CheckGooglePlay()
    {
        // Google Play 開発者サービスの要件を確認する
        var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.
            // app = Firebase.FirebaseApp.DefaultInstance;

            // Set a flag here to indicate whether Firebase is ready to use by your app.
        }
        else
        {
            Debug.LogError(System.String.Format(
              "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
        }
    }

}

