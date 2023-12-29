using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    AudioSource audioSource
    {
        get
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
            return _audioSource;
        }
    }
    AudioSource _audioSource;


    public void PlayOneShot(AudioClip audioClip)
    {
        // if (SaveData.Instance.isOffSE) { return; }
        //if (SoundResourceSO.Instance.resources.Length - 1 < resourceIndex) { return; }
        //AudioClip clip = SoundResourceSO.Instance.resources[resourceIndex].audioClip;
        if (audioClip == null) { return; }
        audioSource.PlayOneShot(audioClip);
    }
}
