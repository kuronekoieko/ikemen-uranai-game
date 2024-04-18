using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create " + nameof(AudioDataSO), fileName = nameof(AudioDataSO))]
public class AudioDataSO : ScriptableObject
{
    public AudioData[] audioDatas;
}

[Serializable]
public class AudioData
{
    public AudioID audioID;
    public AudioClip audioClip;
}

public enum AudioID
{
    None,
    BtnClick_Positive,
    BtnClick_Negative,
    LevelUp,
    HoroscopeScreen,
    Home,
}