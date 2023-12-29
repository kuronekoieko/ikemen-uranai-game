using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenButton_Characters : BaseOpenScreenButton
{
    public override void OnClick()
    {
        PageManager.Instance.Get<CharactersPage>().Open();

    }
    public override void OnStart()
    {
    }
}
