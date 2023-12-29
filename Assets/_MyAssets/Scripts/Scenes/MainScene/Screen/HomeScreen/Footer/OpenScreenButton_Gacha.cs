using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenButton_Gacha : BaseOpenScreenButton
{
    public override void OnClick()
    {
        PageManager.Instance.Get<GachaPage>().Open();
    }

    public override void OnStart()
    {
        Button.interactable = false;
    }
}
