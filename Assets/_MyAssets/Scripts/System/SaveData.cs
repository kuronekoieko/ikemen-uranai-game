using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
[JsonObject]
public class SaveData : BaseSaveData<SaveData>
{
    public Player player = new();
    public int jem_Free;
    public int jem_Charging;

}
