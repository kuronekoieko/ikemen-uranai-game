using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public static class NativeDatePicker
{
    public static void ShowDatePicker(UnityAction<DateTime> callback)
    {
#if UNITY_IOS && !UNITY_EDITOR
      
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidNativePicker.ShowDatePicker((year, month, day) =>
        {
            DateTime dateTime = new DateTime(year, month, day);
            callback(dateTime);
        });
#else
        Debug.Log("Not supported.");
#endif
    }


}
