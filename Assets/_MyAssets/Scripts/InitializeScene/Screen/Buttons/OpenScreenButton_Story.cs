using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenButton_Story : BaseButton
{
    public override void OnClick()
    {
        PageManager.Instance.Get<StoryPage>().Open();

    }


}
