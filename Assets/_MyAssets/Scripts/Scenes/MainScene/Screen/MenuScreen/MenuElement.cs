using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuElement : ObjectPoolingElement
{
    public TextMeshProUGUI titleText;
    [SerializeField] Button button;
    UnityAction onClick;


    public override void OnInstantiate()
    {
        button.onClick.AddListener(OnClick);
    }

    public void Show(MenuElementObj menuElementObj)
    {
        titleText.text = menuElementObj.title;
        this.onClick = menuElementObj.onClick;
    }


    void OnClick()
    {
        onClick?.Invoke();
    }
}
