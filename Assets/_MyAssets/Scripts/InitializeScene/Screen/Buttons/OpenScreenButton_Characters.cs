using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenButton_Characters : BaseButton
{
    public override void OnClick()
    {
        PageManager.Instance.Get<CharactersPage>().Open();

    }

}
