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
        SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
        saveDataManager.LoadSaveData();
        saveData = SaveData.i;
        Debug.Log("セーブデータ取得");
    }

    [Button]
    public void Send()
    {
        SaveData.i = saveData;
        SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
        saveDataManager.Save();
        Debug.Log("セーブデータ更新");
    }

    [Button]
    public void Clear()
    {
        SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
        saveDataManager.Clear();
        saveData = SaveData.i;
        Debug.Log("セーブデータ削除");
    }
}
