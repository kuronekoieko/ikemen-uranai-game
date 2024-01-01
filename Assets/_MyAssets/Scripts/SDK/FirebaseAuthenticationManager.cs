using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Firebase.Auth;

public class FirebaseAuthenticationManager : Singleton<FirebaseAuthenticationManager>
{
    public async UniTask Initialize()
    {
        //Debug.Log("aaaaaaaaaaaaaaa " + User.UserId);
        if (string.IsNullOrEmpty(User.UserId))
        {
            // 毎回新しいユーザーが作成されるっぽい
            await SignInAnonymouslyAsync();
        }
    }

    async UniTask SignInAnonymouslyAsync()
    {

        try
        {
            var firebaseUser = await FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                firebaseUser.DisplayName, firebaseUser.UserId);
            // DebugUtils.LogJson(firebaseUser);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

    }

    public FirebaseUser User => FirebaseAuth.DefaultInstance.CurrentUser;
}
