using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mopsicus.Plugins;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class MenuScreen : BaseScreen
{
    [SerializeField] Button closeButton;
    [SerializeField] Button signInButton;
    [SerializeField] Button signOutButton;
    [SerializeField] Button deleteButton;

    [SerializeField] TextMeshProUGUI uidText;
    [SerializeField] MobileInputField mailAddressMIF;
    [SerializeField] MobileInputField passwordMIF;
    [SerializeField] TextMeshProUGUI signInResultText;



    public override void OnStart()
    {
        base.OnStart();
        closeButton.onClick.AddListener(Close);
        signInButton.onClick.AddListener(async () =>
        {
            bool success = await FirebaseAuthenticationManager.Instance.ReauthenticateAsync(mailAddressMIF.Text, passwordMIF.Text);
            if (success)
            {
                signInResultText.text = "登録成功";
            }
            else
            {
                signInResultText.text = "登録失敗";
            }
            uidText.text = "uid: " + FirebaseAuthenticationManager.Instance.User.UserId;
        });

        signOutButton.onClick.AddListener(() =>
        {
            FirebaseAuthenticationManager.Instance.SignOut();
            uidText.text = "uid: ";
            mailAddressMIF.Text = "";
            passwordMIF.Text = "";
        });

        deleteButton.onClick.AddListener(async () =>
        {
            await FirebaseDatabaseManager.Instance.RemoveSaveData();
            await FirebaseAuthenticationManager.Instance.DeleteAsync();
            FirebaseAuthenticationManager.Instance.SignOut();
            uidText.text = "uid: ";
            mailAddressMIF.Text = "";
            passwordMIF.Text = "";
            await SceneManager.LoadSceneAsync("Initialize");
        });
        // スプラッシュの上に出てしまうため
        mailAddressMIF.SetVisible(false);
        passwordMIF.SetVisible(false);
    }

    public override async void Open()
    {
        base.Open();

        var test = "Voices/chara001-rank001-msg001.wav";
        // var audioClip = await FileDownloader.GetAudioClip(test);
        // Assets/_MyAssets/Audio/Voices/chara0001-rank02-msg01.wav
        //  var url = await FirebaseStorageManager.Instance.GetURI("Assets/iOS/packedassets_assets_all_eb5b8a0c34c9328a6f6b8f39cce08d62.bundle");
        //Debug.Log(url);       ;
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
            mailAddressMIF.Text = FirebaseAuthenticationManager.Instance.User.Email;
            //passwordMIF.Text = FirebaseAuthenticationManager.Instance.User
        }
        signInResultText.text = "";

        mailAddressMIF.SetVisible(true);
        passwordMIF.SetVisible(true);
    }

    public override void Close()
    {
        base.Close();

        mailAddressMIF.SetVisible(false);
        passwordMIF.SetVisible(false);
    }
}
