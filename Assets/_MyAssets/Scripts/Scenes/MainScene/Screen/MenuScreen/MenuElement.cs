using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System;

public class MenuElement : ObjectPoolingElement
{
    public TextMeshProUGUI titleText;
    [SerializeField] Button button;
    Func<string, UniTask> onClick;


    public override void OnInstantiate()
    {
        button.AddListener(OnClick);
    }

    public void Show(MenuElementObj menuElementObj)
    {
        titleText.text = menuElementObj.title;
        this.onClick = menuElementObj.onClick;
    }


    UniTask OnClick()
    {
        return onClick(titleText.text);
    }
}
