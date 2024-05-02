using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] AudioSource soundEffectAS;
    [SerializeField] AudioSource backgroundMusicAS;

    AudioDataSO audioDataSO;
    // readonly string audioDataSO_Compressed = "AudioDataSO_Compressed";
    // readonly string audioDataSO_NotCompressed = "AudioDataSO_NotCompressed";

    public async UniTask Initialize()
    {
        string path = AddressablesLoader.localAddressHeader + "ScriptableObjects/AudioDataSO" + ".asset";
        audioDataSO = await AddressablesLoader.LoadAsync<AudioDataSO>(path);


        if (audioDataSO == null)
        {
            Debug.LogError("AudioManager audioID: " + path + " がありません");
        }
    }

    public void Play(AudioID audioID)
    {
        AudioClip audioClip = GetAudioClip(audioID);
        if (audioClip == null) { return; }
        if (backgroundMusicAS.clip == audioClip) return;

        float duration = 0.5f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => backgroundMusicAS.volume, (x) => backgroundMusicAS.volume = x, 0, duration));
        sequence.AppendCallback(() =>
        {
            backgroundMusicAS.clip = audioClip;
            backgroundMusicAS.Play();
        });
        sequence.Append(DOTween.To(() => backgroundMusicAS.volume, (x) => backgroundMusicAS.volume = x, 1, duration));

    }

    public void PlayOneShot(AudioClip audioClip)
    {
        // if (SaveData.Instance.isOffSE) { return; }
        if (audioClip == null) { return; }
        if (soundEffectAS.clip == audioClip) return;
        soundEffectAS.PlayOneShot(audioClip);
    }

    public void PlayOneShot(AudioID audioID)
    {
        AudioClip audioClip = GetAudioClip(audioID);
        PlayOneShot(audioClip);
    }

    AudioClip GetAudioClip(AudioID audioID)
    {
        if (audioDataSO == null) { return null; }
        if (audioID == AudioID.None) { return null; }

        AudioData audioData = audioDataSO.audioDatas.FirstOrDefault(audioData => audioData.audioID == audioID);
        if (audioData == null)
        {
            Debug.LogError("AudioManager audioID: " + audioID + " がありません");
            return null;
        }
        return audioData.audioClip;
    }
}
