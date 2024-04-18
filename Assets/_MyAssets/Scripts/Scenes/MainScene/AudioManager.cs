using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] AudioSource soundEffectAS;
    [SerializeField] AudioSource backgroundMusicAS;

    AudioDataSO audioDataSO;
    readonly string audioDataSO_Compressed = "AudioDataSO_Compressed";
    readonly string audioDataSO_NotCompressed = "AudioDataSO_NotCompressed";

    public async UniTask Initialize()
    {
        string path = "ScriptableObjects/" + audioDataSO_Compressed;
        // パスに拡張子つけない
        var obj = await Resources.LoadAsync<AudioDataSO>(path);

        audioDataSO = obj as AudioDataSO;
        if (audioDataSO == null)
        {
            Debug.LogError("AudioManager audioID: " + path + " がありません");
        }
    }

    public void Play(AudioID audioID)
    {
        AudioClip audioClip = GetAudioClip(audioID);
        if (audioClip == null) { return; }
        backgroundMusicAS.clip = audioClip;
        backgroundMusicAS.Play();
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        // if (SaveData.Instance.isOffSE) { return; }
        if (audioClip == null) { return; }
        soundEffectAS.PlayOneShot(audioClip);
    }

    public void PlayOneShot(AudioID audioID)
    {
        if (audioDataSO == null) { return; }
        if (audioID == AudioID.None) { return; }

        AudioClip audioClip = GetAudioClip(audioID);

        PlayOneShot(audioClip);
    }

    AudioClip GetAudioClip(AudioID audioID)
    {
        AudioData audioData = audioDataSO.audioDatas.FirstOrDefault(audioData => audioData.audioID == audioID);
        if (audioData == null)
        {
            Debug.LogError("AudioManager audioID: " + audioID + " がありません");
            return null;
        }
        return audioData.audioClip;
    }
}
