using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveFortuneToggle : BaseFooterToggleController
{
    public override void OnStart()
    {
        base.OnStart();
        base.SetSelectedAction(LoveFortuneScreen);
    }
    public void LoveFortuneScreen()
    {
        // PageManager.Instance.Get<HoroscopePage>().Open();
        ScreenManager.Instance.Get<LoveFortuneScreen>().Open();
    }
}
