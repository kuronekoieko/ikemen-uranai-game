using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DateTimeExtensions
{
    public static DateTime? ToNullableDateTime(this string a)
    {
        bool success = DateTime.TryParse(a, out DateTime dateTime);
        if (success)
        {
            return dateTime;
        }
        else
        {
            // Debug.LogWarning("日付のパースに失敗 " + a);
            return null;
        }
    }

    public static DateTime ToDateTime(this string a)
    {
        bool success = DateTime.TryParse(a, out DateTime dateTime);
        if (success)
        {

        }
        else
        {
            Debug.LogWarning("日付のパースに失敗 " + a);
        }

        return dateTime;
    }
}
