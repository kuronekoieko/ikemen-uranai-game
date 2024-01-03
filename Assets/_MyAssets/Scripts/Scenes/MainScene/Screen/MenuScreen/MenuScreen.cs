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

    public override void Open()
    {
        base.Open();

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
