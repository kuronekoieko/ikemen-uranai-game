using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UranaishiProfile_Message : BaseUranaishiProfile
{
    [SerializeField] Text messageTxt;
    [SerializeField] Text belongingTxt;
    [SerializeField] Button twitterButton;
    [SerializeField] Button facebookButton;
    [SerializeField] Button otherURLButton;
    Uranaishi uranaishi;

    public override void OnStart()
    {
        twitterButton.onClick.AddListener(() => Application.OpenURL(uranaishi.twitterURL));
        facebookButton.onClick.AddListener(() => Application.OpenURL(uranaishi.facebookURL));
        otherURLButton.onClick.AddListener(() => Application.OpenURL(uranaishi.otherURL));

    }

    public override void OnOpen(Uranaishi uranaishi)
    {
        this.uranaishi = uranaishi;
        messageTxt.text = uranaishi.message;
        belongingTxt.text = "所属: " + uranaishi.belonging;
        belongingTxt.gameObject.SetActive(!string.IsNullOrEmpty(uranaishi.belonging));

        twitterButton.gameObject.SetActive(!string.IsNullOrEmpty(uranaishi.twitterURL));
        facebookButton.gameObject.SetActive(!string.IsNullOrEmpty(uranaishi.facebookURL));
        otherURLButton.gameObject.SetActive(!string.IsNullOrEmpty(uranaishi.otherURL));

    }
}
