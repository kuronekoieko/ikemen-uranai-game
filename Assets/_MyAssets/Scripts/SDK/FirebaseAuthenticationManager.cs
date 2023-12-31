using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Firebase.Auth;

public class FirebaseAuthenticationManager : Singleton<FirebaseAuthenticationManager>
{
    public async UniTask Initialize()
    {
        var firebaseUser = await FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync();



        Debug.LogFormat("User signed in successfully: {0} ({1})",
            firebaseUser.DisplayName, firebaseUser.UserId);
    }


    public FirebaseUser GetUser()
    {
        return FirebaseAuth.DefaultInstance.CurrentUser;
    }
}
