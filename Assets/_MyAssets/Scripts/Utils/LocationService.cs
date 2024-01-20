using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocationService
{
    // https://docs.unity3d.com/ja/current/ScriptReference/LocationService.Start.html
    public static async UniTask<bool> Start()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("not enabled by user");
            return false;
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
            Debug.Log("Timed out");
            return false;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");

            // Stops the location service if there is no need to query location updates continuously.
            Input.location.Stop();
            return false;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            // Stops the location service if there is no need to query location updates continuously.
            Input.location.Stop();
            return true;
        }

    }
}
