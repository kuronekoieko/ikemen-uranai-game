using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveData
{
    public static SaveData i;

    public List<string> likedUranaishiIdList = new List<string>();
    public int purchasedPoint;
    public int freePoint;
    public List<History> histories = new List<History>();
    public List<Reserve> reserves = new List<Reserve>();
    public Profile myProfile = new Profile();
    public Profile crushProfile = new Profile();

    public SaveData(int freePoint, int purchasedPoint = 0)
    {
        this.purchasedPoint = purchasedPoint;
        this.freePoint = freePoint;
    }

    public int GetAvailableDurationMin(int callChargePerMin)
    {
        float minuteF = (float)GetSumPoint() / (float)callChargePerMin;
        int minuteInt = Mathf.FloorToInt(minuteF);
        return minuteInt;
    }


    public int GetSumPoint()
    {
        return purchasedPoint + freePoint;
    }

    public void ConsumePoints(int consumingPoint)
    {
        //  2000        3000             1000
        int leftPoint = consumingPoint - freePoint;
        if (leftPoint <= 0)
        {
            //2000           1000 
            freePoint -= consumingPoint;
            return;
        }

        freePoint = 0;
        //-3000      2000        5000 
        int lackPoint = leftPoint - purchasedPoint;
        if (lackPoint <= 0)
        {
            //3000           2000
            purchasedPoint -= leftPoint;
            return;
        }
        purchasedPoint = 0;
        Debug.LogError("ポイントが不足しているのに購入できてしまった");

    }

    public Profile GetProfile(bool isMe)
    {
        return isMe ? myProfile : crushProfile;
    }
    public void SetProfile(bool isMe, Profile profile)
    {
        if (isMe)
        {
            myProfile = profile;
        }
        else
        {
            crushProfile = profile;
        }
    }


}

[System.Serializable]
public class History
{
    public SerializableDateTime startCallingSDT;
    public int durationSec;
    public string uranaishiId;

    public History(DateTime startCallingDT, string uranaishiId)
    {
        startCallingSDT = new SerializableDateTime();
        startCallingSDT.dateTime = startCallingDT;
        this.uranaishiId = uranaishiId;
    }
}

[System.Serializable]
public class Reserve
{
    public SerializableDateTime startSDT = new SerializableDateTime();
    public int durationMin;
    public string uranaishiId;
}

[System.Serializable]
public enum BloodType
{
    A = 0,
    B = 1,
    AB = 2,
    O = 3,
    Unknown = 4
}

[System.Serializable]
public class Profile : IEquatable<Profile>
{
    public string firstName;
    public string lastName;
    public SerializableDateTime birthDaySDT = new SerializableDateTime();
    public Sex sex = Sex.None;
    public BloodType bloodType = BloodType.Unknown;

    public bool Equals(Profile other)
    {
        if (other == null)
        {
            return false;
        }

        return firstName == other.firstName &&
            lastName == other.lastName &&
            birthDaySDT.dateTime == other.birthDaySDT.dateTime &&
            sex == other.sex &&
            bloodType == other.bloodType;
    }

    public Profile(Profile profile = null)
    {
        if (profile == null) return;
        firstName = profile.firstName;
        lastName = profile.lastName;
        birthDaySDT.dateTime = profile.birthDaySDT.dateTime;
        sex = profile.sex;
        bloodType = profile.bloodType;
    }
}