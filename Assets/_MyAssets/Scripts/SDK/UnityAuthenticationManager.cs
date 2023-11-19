using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
//using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class UnityAuthenticationManager : Singleton<UnityAuthenticationManager>
{

    public async Task Initialize()
    {
        await UnityServices.InitializeAsync();
        await SignInAnonymouslyAsync();
    }

    public string GetPlayerId()
    {
        return AuthenticationService.Instance.PlayerId;
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
