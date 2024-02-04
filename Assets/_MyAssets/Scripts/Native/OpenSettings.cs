using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
#endif

public static class OpenSettings 
{
#if !UNITY_EDITOR && UNITY_IOS
    [DllImport("__Internal")]
    public static extern void Open();
#else
    public static void Open()
    {
    }
#endif
}
