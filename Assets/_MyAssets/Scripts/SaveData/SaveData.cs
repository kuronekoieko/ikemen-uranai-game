using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using SaveDataObjects;

[Serializable]
[JsonObject]
public class SaveData
{
    public Player player = new();
    public int jemFree;
    public int jemCharging;
    public string currentCharacterId = "0001";
    // 配列は使わない。dicにする→データ更新のときに上書きされずに、要素が追加されるため
    // 初期値nullにするとセーブデータがnullで上書きされる
    public Dictionary<string, Character> characters = new();
}
