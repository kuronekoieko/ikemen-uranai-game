using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class BaseButton : MonoBehaviour
{
    protected Button Button
    {
        get
        {
            if (_button == null) _button = GetComponent<Button>();
            return _button;
        }
    }
    Button _button;

    void Awake()
    {
        //Button.AddListener(OnClick);
    }

    public abstract void OnClick();
}