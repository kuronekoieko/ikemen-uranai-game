using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    public static T Instance => instance;
    private static T instance = new();
}
