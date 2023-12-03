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
        FirebaseDatabaseManager.Instance.Initialize();
        uranaishis = await FirebaseDatabaseManager.Instance.GetUranaishiAry(10);
        Debug.Log("データベース取得");

    }

    [Button]
    public async void Send()
    {
        FirebaseDatabaseManager.Instance.Initialize();

        foreach (var uranaishi in uranaishis)
        {
            await FirebaseDatabaseManager.Instance.SetUserData(uranaishi);
        }
        Debug.Log("データベース更新");
    }
}
