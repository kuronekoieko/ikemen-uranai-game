using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaToggle : BaseFooterToggleController
{
    public override void OnStart()
    {
        base.OnStart();
        base.SetSelectedAction(Gacha);
    }
    public void Gacha()
    {
        PageManager.Instance.Get<GachaPage>().Open();
    }
}
