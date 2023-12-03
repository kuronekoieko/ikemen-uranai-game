using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AboutAppModal : BaseModal
{
    [SerializeField] Button privacyPolicyButton;
    [SerializeField] Button tokushouhouButton;
    [SerializeField] Button licenseButton;
    [SerializeField] Button cancelMembershipButton;
    [SerializeField] TextMeshProUGUI userIdText;
    [SerializeField] TextMeshProUGUI appVersionText;

    public override void OnStart()
    {
        base.OnStart();

        privacyPolicyButton.onClick.AddListener(OnClickPrivacyPolicyButton);
        tokushouhouButton.onClick.AddListener(OnClickTokushouhouButton);
        licenseButton.onClick.AddListener(OnClickLicenseButton);
        cancelMembershipButton.onClick.AddListener(OnClickCancelMembershipButton);
        userIdText.text = "仮のユーザーID";// TODO
        appVersionText.text = Application.version.ToString();
    }

    public void Open()
    {
        base.OpenAnim();
    }

    void OnClickPrivacyPolicyButton()
    {
        ChromeCustomTabs.OpenURL("");
    }

    void OnClickTokushouhouButton()
    {
        ChromeCustomTabs.OpenURL("");
    }
    void OnClickLicenseButton()
    {
        ChromeCustomTabs.OpenURL("");
    }
    void OnClickCancelMembershipButton()
    {
        ChromeCustomTabs.OpenURL("");
    }
}
