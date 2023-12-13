using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreen : BaseScreen
{
    [SerializeField] GameObject character;
    public override void OnStart()
    {
        base.OnStart();
        //character.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
        // character.SetActive(true);

    }

    public override void Close()
    {
        base.Close();
        // character.SetActive(false);
    }
}
