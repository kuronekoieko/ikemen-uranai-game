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
    public int level = 1;
    public int exp = 0;
    public int jemFree;
    public int jemCharging;
    public string birthDay = "01/01";
    public DateTime BirthDayDT => birthDay.ToDateTime();
    public string currentCharacterId = "0001";
    // 配列は使わない。dicにする→データ更新のときに上書きされずに、要素が追加されるため
    // 初期値nullにするとセーブデータがnullで上書きされる
    public Dictionary<string, Character> characters = new();

    public DataBase.Constellation Constellation
    {
        get
        {
            _Constellation ??= GetConstellation(BirthDayDT);
            return _Constellation;
        }
    }
    [NonSerialized] DataBase.Constellation _Constellation;


    public DataBase.Constellation GetConstellation(DateTime birthDayDT)
    {
        // Debug.Log(birthDayDT);

        var orderedConstellations = CSVManager.Instance.Constellations
            .OrderBy(c => (birthDayDT - c.StartDT).Days).ToArray();

        // startとendの間だと山羊座のときにnull
        var constellation = orderedConstellations
            .FirstOrDefault(c => (birthDayDT - c.StartDT).Days >= 0);
        if (constellation == null)
        {
            constellation = orderedConstellations
                .FirstOrDefault(c => (c.EndDT - birthDayDT).Days >= 0);
        }

        return constellation;
    }
}
