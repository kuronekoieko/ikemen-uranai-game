using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<SingletonType> where SingletonType : Singleton<SingletonType>, new()
{
    public static SingletonType i => _i;
    static SingletonType _i = new SingletonType();
}
