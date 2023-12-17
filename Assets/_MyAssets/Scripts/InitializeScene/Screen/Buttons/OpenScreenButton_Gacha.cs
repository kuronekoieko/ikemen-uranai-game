using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenButton_Gacha : BaseButton
{
    public override void OnClick()
    {
        PageManager.Instance.Get<GachaPage>().Open();

    }

}
