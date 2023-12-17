using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenButton_Home : BaseButton
{
    public override void OnClick()
    {
        PageManager.Instance.Get<HomePage>().Open();
    }


}
