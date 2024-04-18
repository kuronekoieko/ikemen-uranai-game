using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] AudioSource audioSource;


    AudioDataSO audioDataSO;

    public async UniTask Initialize()
    {
        // パスに拡張子つけない
        var obj = await Resources.LoadAsync<AudioDataSO>("ScriptableObjects/AudioDataSO");
        audioDataSO = obj as AudioDataSO;
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        // if (SaveData.Instance.isOffSE) { return; }
        if (audioClip == null) { return; }
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayOneShot(AudioID audioID)
    {
        if (audioDataSO == null) { return; }
        AudioData audioData = audioDataSO.audioDatas.FirstOrDefault(audioData => audioData.audioID == audioID);
        if (audioData == null)
        {
            Debug.LogError("AudioManager audioID: " + audioID + " がありません");
            return;
        }

        PlayOneShot(audioData.audioClip);
    }
}
