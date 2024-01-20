using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
