using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUtils
{
    public static void LogJson(object msg)
    {
        Debug.Log(JsonUtility.ToJson(msg, true));
    }

    public static void Log(object[] msg)
    {
        string str = "";

        foreach (var item in msg)
        {
            str += item + " ";
        }

        Debug.Log(str);
    }
}
