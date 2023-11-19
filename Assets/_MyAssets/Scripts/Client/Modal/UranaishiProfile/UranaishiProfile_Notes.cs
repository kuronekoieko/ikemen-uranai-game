using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UranaishiProfile_Notes : BaseUranaishiProfile
{
    [SerializeField] Text text;
    [SerializeField] Button privacyPolicyButton;
    public override void OnStart()
    {
        privacyPolicyButton.onClick.AddListener(() =>
        {
            ChromeCustomTabs.OpenURL("https://www.google.co.jp");
        });
    }
    public override void OnOpen(Uranaishi uranaishi)
    {
    }
}
