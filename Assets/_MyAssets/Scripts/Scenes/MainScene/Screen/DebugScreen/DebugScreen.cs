using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mopsicus.Plugins;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class DebugScreen : BaseScreen
{
    [SerializeField] Button signInButton;
    [SerializeField] Button signOutButton;
    [SerializeField] Button deleteButton;

    [SerializeField] TextMeshProUGUI uidText;
    [SerializeField] MobileInputField mailAddressMIF;
    [SerializeField] MobileInputField passwordMIF;
    [SerializeField] TextMeshProUGUI signInResultText;



    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
        signInButton.AddListener(async () =>
        {
            bool success = await FirebaseAuthenticationManager.ReauthenticateAsync(mailAddressMIF.Text, passwordMIF.Text);
            if (success)
            {
                signInResultText.text = "登録成功";
            }
            else
            {
                signInResultText.text = "登録失敗";
            }
            uidText.text = "uid: " + FirebaseAuthenticationManager.User.UserId;
        });

        signOutButton.AddListener(() =>
        {
            FirebaseAuthenticationManager.SignOut();
            uidText.text = "uid: ";
            mailAddressMIF.Text = "";
            passwordMIF.Text = "";
            return UniTask.DelayFrame(0);
        });

        deleteButton.AddListener(OnClickDeleteButton);
        // スプラッシュの上に出てしまうため
        mailAddressMIF.SetVisible(false);
        passwordMIF.SetVisible(false);
    }

    async UniTask OnClickSignInButton()
    {
        bool success = await FirebaseAuthenticationManager.ReauthenticateAsync(mailAddressMIF.Text, passwordMIF.Text);
        if (success)
        {
            signInResultText.text = "登録成功";
        }
        else
        {
            signInResultText.text = "登録失敗";
        }
        uidText.text = "uid: " + FirebaseAuthenticationManager.User.UserId;
    }

    async UniTask OnClickDeleteButton()
    {
        await FirebaseDatabaseManager.RemoveSaveData();
        await FirebaseAuthenticationManager.DeleteAsync();
        FirebaseAuthenticationManager.SignOut();
        uidText.text = "uid: ";
        mailAddressMIF.Text = "";
        passwordMIF.Text = "";
        // await SceneManager.LoadSceneAsync("Initialize");
        Quit();
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
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
        var audioClip = await AddressablesLoader.LoadAsync<AudioClip>(localPath + test);
        DebugUtils.LogJson(audioClip);

        AudioManager.Instance.PlayOneShot(audioClip);

        if (FirebaseAuthenticationManager.User == null)
        {
            uidText.text = "uid: ";
        }
        else
        {
            uidText.text = "uid: " + FirebaseAuthenticationManager.User.UserId;
            mailAddressMIF.Text = FirebaseAuthenticationManager.User.Email;
            //passwordMIF.Text = FirebaseAuthenticationManager.Instance.User
        }
        signInResultText.text = "";

        mailAddressMIF.SetVisible(true);
        passwordMIF.SetVisible(true);
    }

    public override UniTask Close()
    {
        base.Close();

        mailAddressMIF.SetVisible(false);
        passwordMIF.SetVisible(false);
        return UniTask.DelayFrame(0);
    }
}
