using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using SaveDataObjects;

[Serializable]
[JsonObject]
public class SaveData : BaseSaveData<SaveData>
{
    public Player player = new();
    public int jemFree;
    public int jemCharging;
    public string currentCharacterId = "0001";
    public List<Character> characters = new();

}
