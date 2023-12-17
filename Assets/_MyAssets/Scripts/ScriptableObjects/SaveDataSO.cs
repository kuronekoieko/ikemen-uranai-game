using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "MyGame/Create " + nameof(SaveDataSO), fileName = nameof(SaveDataSO))]
public class SaveDataSO : ScriptableObject
{
    [SerializeField] SaveData saveData;




    [Button]
    public void Pull()
    {
        saveData.LoadSaveData();
        Debug.Log("セーブデータ取得");
    }

    [Button]
    public void Send()
    {
        saveData.Save();
        Debug.Log("セーブデータ更新");
    }

    [Button]
    public void Clear()
    {
        saveData.LoadSaveData();
        saveData.Clear();
        Debug.Log("セーブデータ削除");
    }
}
