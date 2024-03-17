using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryToggle : BaseFooterToggleController
{
    public override void OnStart()
    {
        base.OnStart();
        base.SetSelectedAction(Story);
    }
    public void Story()
    {
        PageManager.Instance.Get<StoryPage>().Open();
    }
}
