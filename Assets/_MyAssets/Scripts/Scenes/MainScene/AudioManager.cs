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
        var obj = await Resources.LoadAsync<AudioDataSO>("ScriptableObjects/AudioDataSO.asset");
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
        AudioClip audioClip = audioDataSO.audioDatas.FirstOrDefault(audioData => audioData.audioID == audioID).audioClip;
        PlayOneShot(audioClip);
    }
}
