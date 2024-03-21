using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocationService
{
    // https://docs.unity3d.com/ja/current/ScriptReference/LocationService.Start.html
    public static async void Start()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("LocationService: " + "not enabled by user");
            OpenSettings.Open();
            return;
        }

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        UniTask initializingTask = UniTask.WaitWhile(() => Input.location.status == LocationServiceStatus.Initializing);
        UniTask timeOutTask = UniTask.Delay(1000 * 20);
        var result = await UniTask.WhenAny(initializingTask, timeOutTask);

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (result == 1)
        {
            Debug.Log("LocationService: " + "Timed out");
            return;
        }

        Debug.Log("LocationService: " + Input.location.status);

        switch (Input.location.status)
        {
            case LocationServiceStatus.Stopped:
                break;
            case LocationServiceStatus.Running:
                break;
            case LocationServiceStatus.Failed:
                OpenSettings.Open();
                break;
            default:
                break;
        }
        Input.location.Stop();
        Debug.Log("LocationService: " + Input.location.status);
    }
}
