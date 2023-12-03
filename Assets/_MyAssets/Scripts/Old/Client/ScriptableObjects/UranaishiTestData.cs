using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "MyGame/Create " + nameof(UranaishiTestData), fileName = nameof(UranaishiTestData))]
public class UranaishiTestData : ScriptableObject
{

    [ListDrawerSettings
    (
        DraggableItems = false,
        Expanded = false,
        ShowIndexLabels = true,
        ShowPaging = false,
        ShowItemCount = true
    )]
    public Uranaishi[] uranaishis;

    [Button]
    public async void Pull()
    {
        FirebaseDatabaseManager.i.Initialize();
        uranaishis = await FirebaseDatabaseManager.i.GetUranaishiAry(10);
        Debug.Log("データベース取得");

    }

    [Button]
    public async void Send()
    {
        FirebaseDatabaseManager.i.Initialize();

        foreach (var uranaishi in uranaishis)
        {
            await FirebaseDatabaseManager.i.SetUserData(uranaishi);
        }
        Debug.Log("データベース更新");
    }
}
