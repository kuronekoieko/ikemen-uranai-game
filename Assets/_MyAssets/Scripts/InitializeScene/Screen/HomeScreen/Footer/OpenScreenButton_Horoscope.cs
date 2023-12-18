using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenButton_Horoscope : BaseOpenScreenButton
{
    public override void OnClick()
    {
        PageManager.Instance.Get<HoroscopePage>().Open();

    }

}
