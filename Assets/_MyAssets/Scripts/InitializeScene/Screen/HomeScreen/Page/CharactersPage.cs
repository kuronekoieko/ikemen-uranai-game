using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersPage : BasePage
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
        Debug.Log("子");
    }
}
