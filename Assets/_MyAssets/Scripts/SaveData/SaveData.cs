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
    public int userNumber;
    public string displayUserId;
    public string firebaseUserId;
    public string name;
    public int level = 1;
    public int exp = 0;
    public int jemFree;
    public int jemCharging;
    public string birthDay;
    public int currentCharacterId = 1;
    public Notification notification = new();
    public string lastLoginDateTime = "";
    // 配列は使わない。dicにする→データ更新のときに上書きされずに、要素が追加されるため
    // 初期値nullにするとセーブデータがnullで上書きされる
    // キーをintにすると、自動的に配列になってしまう
    // キーをstringにしても、0埋めしないと勝手に配列になる
    public Dictionary<string, Character> characters = new();
    public Dictionary<string, HoroscopeHistory> horoscopeHistories = new();



    [JsonIgnore]
    public DateTime? BirthDayDT => birthDay.ToNullableDateTime();

    [JsonIgnore]
    public DataBase.Constellation Constellation => GetConstellation(BirthDayDT);

    public Character GetCurrentCharacter()
    {
        return GetCharacter(currentCharacterId);
    }

    public Character GetCharacter(int characterId)
    {
        foreach (var pair in characters)
        {
            if (pair.Value.id == characterId)
            {
                return pair.Value;
            }
        }
        return null;
    }

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
