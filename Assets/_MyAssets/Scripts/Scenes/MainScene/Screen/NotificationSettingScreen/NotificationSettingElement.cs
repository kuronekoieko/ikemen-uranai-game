using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationSettingElement : ObjectPoolingElement
{
    public TextMeshProUGUI titleText;
    public Toggle toggle;
    UnityAction<bool> onValueChanged;


    public override void OnInstantiate()
    {
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    public void Show(NotificationSettingObj notificationSettingObj)
    {
        titleText.text = notificationSettingObj.title;
        onValueChanged = notificationSettingObj.onValueChanged;
    }


    void OnValueChanged(bool isOn)
    {
        onValueChanged(isOn);
    }
}
