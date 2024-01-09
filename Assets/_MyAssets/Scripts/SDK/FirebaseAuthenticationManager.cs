using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Firebase.Auth;

public class FirebaseAuthenticationManager : Singleton<FirebaseAuthenticationManager>
{
    public FirebaseUser User => FirebaseAuth.DefaultInstance.CurrentUser;

    public async UniTask Initialize()
    {
        if (User == null)
        {
            // 毎回新しいユーザーが作成されるっぽい？
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

    public void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }

    public async UniTask<bool> ReauthenticateAsync(string mailAddress, string password)
    {

        // Get auth credentials from the user for re-authentication. The example below shows
        // email and password credentials but there are multiple possible providers,
        // such as GoogleAuthProvider or FacebookAuthProvider.
        Credential credential = EmailAuthProvider.GetCredential(mailAddress, password);

        bool success = false;
        if (User == null) return success;

        await User.ReauthenticateAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("ReauthenticateAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("ReauthenticateAsync encountered an error: " + task.Exception);
                return;
            }
            success = true;
            Debug.Log("User reauthenticated successfully.");
        });

        return success;
    }


    public async UniTask DeleteAsync()
    {
        if (User == null) return;

        await User.DeleteAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("DeleteAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("User deleted successfully.");
        });

    }

}
