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
        SaveDataManager.Load();
        saveData = SaveDataManager.SaveData;
        Debug.Log("セーブデータ取得");
    }

    [Button]
    public void Send()
    {
        SaveDataManager.Replace(saveData);
        Debug.Log("セーブデータ更新");
    }

    [Button]
    public void Clear()
    {
        SaveDataManager.Clear();
        saveData = SaveDataManager.SaveData;
        Debug.Log("セーブデータ削除");
    }
}
