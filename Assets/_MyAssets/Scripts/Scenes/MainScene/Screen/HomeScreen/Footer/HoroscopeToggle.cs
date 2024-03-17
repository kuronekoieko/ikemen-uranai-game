using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoroscopeToggle : BaseFooterToggleController
{
    public override void OnStart()
    {
        base.OnStart();
        base.SetSelectedAction(Horoscope);
    }
    public void Horoscope()
    {
        PageManager.Instance.Get<HoroscopePage>().Open();
    }
}

