using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class OpenScreenButton_Home : BaseOpenScreenButton
{

    public override void OnClick()
    {
        PageManager.Instance.Get<HomePage>().Open();
    }


}