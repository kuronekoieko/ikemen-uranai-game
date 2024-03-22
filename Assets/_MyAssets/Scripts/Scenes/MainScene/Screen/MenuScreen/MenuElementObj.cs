using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class MenuElementObj
{
    public string title;
    public Func<string, UniTask> onClick;
}
