using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : BaseScreen
{
    [SerializeField] Button closeButton;
    [SerializeField] Button signInButton;
    [SerializeField] Button signOutButton;
    [SerializeField] TextMeshProUGUI uidText;

    public override void OnStart()
    {
        base.OnStart();
        closeButton.onClick.AddListener(Close);
        signInButton.onClick.AddListener(async () =>
        {
            await FirebaseAuthenticationManager.Instance.Initialize();
            uidText.text = "uid: " + FirebaseAuthenticationManager.Instance.User.UserId;
        });
        signOutButton.onClick.AddListener(async () =>
        {
            await FirebaseDatabaseManager.Instance.RemoveSaveData();
            await FirebaseAuthenticationManager.Instance.DeleteAsync();
            FirebaseAuthenticationManager.Instance.SignOut();
            uidText.text = "uid: ";
        });

    }

    public override async void Open()
    {
        base.Open();

        var test = "Voices/chara001-rank001-msg001.wav";
        // var audioClip = await FileDownloader.GetAudioClip(test);
        // Assets/_MyAssets/Audio/Voices/chara0001-rank02-msg01.wav
        var localPath = "Assets/_MyAssets/Audio/";
        var audioClip = await AssetBundleLoader.LoadAddressablesAsync<AudioClip>(localPath + test);
        DebugUtils.LogJson(audioClip);

        AudioManager.Instance.PlayOneShot(audioClip);

        if (FirebaseAuthenticationManager.Instance.User == null)
        {
            uidText.text = "uid: ";
        }
        else
        {
            uidText.text = "uid: " + FirebaseAuthenticationManager.Instance.User.UserId;
        }
    }

    public override void Close()
    {
        base.Close();
    }
}
