using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class DebugUtils
{
    public static void LogJson(object msg)
    {
        Debug.Log(JsonConvert.SerializeObject(msg, Formatting.Indented));
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
