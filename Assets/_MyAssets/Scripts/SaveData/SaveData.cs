using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using SaveDataObjects;
using Cysharp.Threading.Tasks;
using System.Linq;

[Serializable]
[JsonObject]
public class SaveData
{
    public string uid = "789123";
    public string name;
    public int level = 1;
    public int exp = 0;
    public int jemFree;
    public int jemCharging;
    public string birthDay;
    public string currentCharacterId = "0001";
    // 配列は使わない。dicにする→データ更新のときに上書きされずに、要素が追加されるため
    // 初期値nullにするとセーブデータがnullで上書きされる
    public Dictionary<string, Character> characters = new();



    public DateTime? BirthDayDT => birthDay.ToNullableDateTime();

    public DataBase.Constellation Constellation => GetConstellation(BirthDayDT);


    public DataBase.Constellation GetConstellation(DateTime? birthDayDT)
    {
        // Debug.Log(birthDayDT);
        if (birthDayDT == null) return null;

        var orderedConstellations = CSVManager.Instance.Constellations
            .OrderBy(c => (birthDayDT.Value - c.StartDT.Value).Days).ToArray();

        // startとendの間だと山羊座のときにnull
        var constellation = orderedConstellations
            .FirstOrDefault(c => (birthDayDT.Value - c.StartDT.Value).Days >= 0);
        if (constellation == null)
        {
            constellation = orderedConstellations
                .FirstOrDefault(c => (c.EndDT.Value - birthDayDT.Value).Days >= 0);
        }

        return constellation;
    }
}
