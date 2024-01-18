using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotificationSettingObj
{
    public string title;
    public UnityAction<bool> onValueChanged;
    public bool isOn;// 起動時のみ
}
